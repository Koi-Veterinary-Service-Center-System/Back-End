using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Payment;
using KoiFishCare.DTOs.Payment;
using KoiFishCare.Interfaces;
using KoiFishCare.Mappers;
using KoiFishCare.service.VnpayService.Models;
using KoiFishCare.service.VnpayService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Text.RegularExpressions;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using KoiFishCare.Models;


namespace KoiFishCare.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IVnPayService _vnPayService;
        private readonly IBookingRepository _bookingRepo;
        private readonly IEmailService _emailService;
        public PaymentController(IPaymentRepository paymentRepo, IVnPayService vnPayService,
        IBookingRepository bookingRepo, IEmailService emailService)
        {
            _paymentRepo = paymentRepo;
            _vnPayService = vnPayService;
            _bookingRepo = bookingRepo;
            _emailService = emailService;
        }

        [HttpGet("all-payment")]
        public async Task<IActionResult> GetAllPayment()
        {
            var payments = await _paymentRepo.GetAllPayment();
            if (payments == null || !payments.Any()) return BadRequest("Can not find any payment");
            return Ok(payments.Select(x => x.ToDTOFromModel()));
        }

        // [HttpPost("create-payment")]
        // [Authorize(Roles = "Manager, Staff")]
        // public async Task<IActionResult> AddPayment([FromBody] AddPaymentDTO addPaymentDTO)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(ModelState);
        //     }

        //     if (addPaymentDTO.Type.Length < 3)
        //     {
        //         return BadRequest("The length of the type must be greater than or equal to 3!");
        //     }

        //     var payment = new Payment();
        //     payment.Type = addPaymentDTO.Type;
        //     await _paymentRepo.Add(payment);
        //     return Ok(payment.ToDTOFromModel());
        // }

        // [HttpPut("update-payment/{id:int}")]
        // [Authorize(Roles = "Manager")]
        // public async Task<IActionResult> UpdatePayment([FromRoute] int id, [FromBody] AddPaymentDTO updatePaymentDTO)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(ModelState);
        //     }

        //     var paymentModel = await _paymentRepo.GetPaymentByID(id);
        //     if (paymentModel == null)
        //     {
        //         return BadRequest("Can not find any payment!");
        //     }

        //     if (updatePaymentDTO.Type.Length < 3)
        //     {
        //         return BadRequest("The length of the type must be greater than or equal to 3!");
        //     }

        //     paymentModel.Type = updatePaymentDTO.Type;
        //     await _paymentRepo.Update(paymentModel);
        //     return Ok(paymentModel.ToDTOFromModel());
        // }

        // [HttpDelete("delete-payment/{id:int}")]
        // [Authorize(Roles = "Manager")]
        // public async Task<IActionResult> DeletePayment([FromRoute] int id)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         return BadRequest(ModelState);
        //     }

        //     var paymentModel = await _paymentRepo.GetPaymentByID(id);
        //     if (paymentModel == null)
        //     {
        //         return BadRequest("Can not find any payment!");
        //     }

        //     await _paymentRepo.Delete(paymentModel);
        //     return Ok("Deleted successfully!");
        // }

        [HttpPost("create-paymentUrl")]
        public async Task<IActionResult> CreatePaymentUrl(int bookingId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var booking = await _bookingRepo.GetBookingByIdAsync(bookingId);
            if (booking == null)
                return NotFound("Invalid bookingId");

            if (booking.Payment.Type.Contains("cash") || booking.Payment.Type.Contains("Cash"))
                return BadRequest("Cash payment cannot use this!");

            var model = new PaymentInformationModel()
            {
                BookingID = booking.BookingID,
                Amount = booking.InitAmount,
                ServiceName = booking.Service.ServiceName
            };

            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Ok(new { PaymentUrl = url });
        }

        [HttpGet("paymentCallback")]
        public async Task<IActionResult> PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response == null || response.OrderId == null)
                return Redirect("http://localhost:5173/paymentfailed");

            if (!TryParseBookingId(response.OrderId, out int bookingId))
                return Redirect("http://localhost:5173/paymentfailed");

            var booking = await _bookingRepo.GetBookingByIdAsync(bookingId);
            if (booking == null)
            {
                return Redirect("http://localhost:5173/paymentfailed");
            }

            if (response.VnPayResponseCode != "00")
            {
                _bookingRepo.DeleteBooking(booking);
                return Redirect("http://localhost:5173/paymentfailed");
            }

            booking.BookingStatus = Models.Enum.BookingStatus.Scheduled;
            booking.isPaid = true;
            await _bookingRepo.UpdateBooking(booking);

            // Compose and send an email notification with payment information
            var htmlContent = $@"
<html>
  <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
    <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 10px; padding: 30px; box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);'>
      <div style='text-align: center;'>
        <img src='https://firebasestorage.googleapis.com/v0/b/swp391veterinary.appspot.com/o/logo.png?alt=media&token=a26711fc-ed75-4e62-8af1-ec577334574a' alt='KoiNe Logo' style='width: 120px; margin-bottom: 20px;' />
        <h2 style='color: #4A90E2; font-size: 24px; margin: 0;'>Booking Payment Confirmation</h2>
      </div>
      
      <p style='color: #333333; font-size: 16px; line-height: 1.6; margin-top: 20px;'>Dear {booking.Customer.UserName},</p>
      
      <p style='color: #333333; font-size: 16px; line-height: 1.6;'>We are pleased to inform you that your booking payment has been successfully processed. Here are the details of your booking:</p>
      
      <div style='margin: 20px 0; padding: 15px; background-color: #e6f7ff; border-left: 4px solid #4A90E2; border-radius: 5px;'>
        <p style='color: #333333; font-size: 16px; margin: 5px 0;'><strong>Booking ID:</strong> {booking.BookingID}</p>
        <p style='color: #333333; font-size: 16px; margin: 5px 0;'><strong>Booking Date:</strong> {booking.BookingDate.ToString("dd MMMM yyyy")}</p>
        <p style='color: #333333; font-size: 16px; margin: 5px 0;'><strong>Veterinarian:</strong> Dr. {booking.Veterinarian.FirstName} {booking.Veterinarian.LastName}</p>
      </div>
      
      <p style='color: #333333; font-size: 16px; line-height: 1.6;'>If you have any questions or require further assistance, please feel free to <a href='mailto:support@KoiNe.com' style='color: #1d72b8; text-decoration: none;'>contact our support team</a>.</p>
      
      <hr style='border: none; border-top: 1px solid #eeeeee; margin: 20px 0;' />
      
      <p style='color: #777777; font-size: 12px; text-align: center;'>Thank you for trusting us with your pet's care. We look forward to serving you and your pet!</p>
      
      <p style='font-size: 12px; text-align: center; color: #777777; margin-top: 20px;'>&copy; 2024 KoiNe. All rights reserved.</p>
    </div>
  </body>
</html>";
            await _emailService.SendEmailAsync(booking.Customer.Email!, "Booking Payment Notification", htmlContent);

            return Redirect($"http://localhost:5173/paymentsuccess/{bookingId}");
        }

        private bool TryParseBookingId(string orderId, out int bookingId)
        {
            bookingId = 0; // Assign a default value to bookingId
            var txnRefParts = orderId.Split('-');
            return txnRefParts.Length > 0 && int.TryParse(txnRefParts[0], out bookingId);
        }


        [HttpPatch("soft-delete/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> SoftDeletePayment([FromRoute] int id)
        {
            var existingPayment = await _paymentRepo.GetPaymentByID(id);
            if (existingPayment == null)
            {
                return NotFound("Can not find this payment!");
            }

            existingPayment.IsDeleted = true;
            await _paymentRepo.Update(existingPayment);
            return Ok("Deleted successfully!");
        }
    }
}

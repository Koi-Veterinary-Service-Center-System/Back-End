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

namespace KoiFishCare.Controllers
{
    [Route("api/payment")]
    [ApiController]
    [AllowAnonymous]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IVnPayService _vnPayService;
        private readonly IBookingRepository _bookingRepo;
        public PaymentController(IPaymentRepository paymentRepo, IVnPayService vnPayService, IBookingRepository bookingRepo)
        {
            _paymentRepo = paymentRepo;
            _vnPayService = vnPayService;
            _bookingRepo = bookingRepo;
        }

        [HttpGet("all-payment")]
        public async Task<IActionResult> GetAllPayment()
        {
            var payments = await _paymentRepo.GetAllPayment();
            if (payments == null || !payments.Any()) return BadRequest("Can not find any payment");

            var paymentsDto = payments.Select(p => new PaymentDTO
            {
                PaymentID = p.PaymentID,
                Qrcode = p.Qrcode,
                Type = p.Type
            }).ToList();

            return Ok(paymentsDto);
        }

        [HttpPost("create-payment")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> AddPayment([FromBody] AddPaymentDTO addPaymentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (addPaymentDTO.Type.Length < 3)
            {
                return BadRequest("The length of the type must be greater than or equal to 3!");
            }

            await _paymentRepo.Add(addPaymentDTO.ToModelFromDTO());
            return Ok(addPaymentDTO);
        }

        [HttpPut("update-payment/{id:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdatePayment([FromRoute] int id, [FromBody] AddPaymentDTO updatePaymentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paymentModel = await _paymentRepo.GetPaymentByID(id);
            if (paymentModel == null)
            {
                return BadRequest("Can not find any payment!");
            }

            if (updatePaymentDTO.Type.Length < 3)
            {
                return BadRequest("The length of the type must be greater than or equal to 3!");
            }

            await _paymentRepo.Update(updatePaymentDTO.ToModelFromDTO());
            return Ok(updatePaymentDTO);
        }

        [HttpDelete("delete-payment/{id:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeletePayment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paymentModel = await _paymentRepo.GetPaymentByID(id);
            if (paymentModel == null)
            {
                return BadRequest("Can not find any payment!");
            }

            await _paymentRepo.Delete(paymentModel);
            return Ok("Deleted successfully!");
        }

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

            var model = new PaymentInformationModel()
            {
                BookingID = booking.BookingID,
                Amount = booking.TotalAmount,
                ServiceName = booking.Service.ServiceName
            };

            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Ok(new { PaymentUrl = url });
        }

        [HttpGet("paymentCallback")]
        public async Task<IActionResult> PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                return BadRequest(new
                {
                    Message = $"VN Pay payment failed, response code: {response?.VnPayResponseCode ?? "No response"}"
                });
            }

            if (int.TryParse(response.OrderId, out int bookingId))
            {
                var booking = await _bookingRepo.GetBookingByIdAsync(bookingId);
                if (booking != null)
                {
                    booking.BookingStatus = Models.Enum.BookingStatus.Scheduled;
                    _bookingRepo.UpdateBooking(booking);

                    return Redirect($"http://localhost:5173/paymentsuccess/{bookingId}");
                }
                else
                {
                    return NotFound(new
                    {
                        Message = "Booking not found"
                    });
                }
            }
            else
            {
                return BadRequest(new
                {
                    Message = "Invalid booking ID"
                });
            }
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

            existingPayment.isDeleted = true;
            await _paymentRepo.Update(existingPayment);
            return Ok("Deleted successfully!");
        }
    }
}

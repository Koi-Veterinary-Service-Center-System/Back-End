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
        public PaymentController(IPaymentRepository paymentRepo, IVnPayService vnPayService)
        {
            _paymentRepo = paymentRepo;
            _vnPayService = vnPayService;
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

        [HttpPut("update-payment/{paymentID:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdatePayment([FromRoute] int paymentID, [FromBody] AddPaymentDTO updatePaymentDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paymentModel = await _paymentRepo.GetPaymentByID(paymentID);
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

        [HttpDelete("delete-payment/{paymentID:int}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeletePayment([FromRoute] int paymentID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paymentModel = await _paymentRepo.GetPaymentByID(paymentID);
            if (paymentModel == null)
            {
                return BadRequest("Can not find any payment!");
            }

            await _paymentRepo.Delete(paymentModel);
            return Ok("Deleted successfully!");
        }

        [HttpPost("create-paymentUrl")]
        public IActionResult CreatePaymentUrl([FromBody] PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            // Thay vì redirect, trả về URL dưới dạng JSON
            return Ok(new { PaymentUrl = url });
        }

        [HttpGet("paymentCallback")]
        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            if (response == null || response.VnPayResponseCode != "00")
            {
                return BadRequest(new
                {
                    Message = $"VN Pay payment failed, response code: {response?.VnPayResponseCode ?? "No response"}"
                });
            }

            
            return Ok(new
            {
                Message = "VN Pay payment succeeded"
            });

            // return Ok(response); // Trả về kết quả dưới dạng JSON
        }

    }
}

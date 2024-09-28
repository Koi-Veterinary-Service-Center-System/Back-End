using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.DTOs.Payment;
using KoiFishCare.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishCare.Controllers
{
    [Route("api/payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepo;
        public PaymentController(IPaymentRepository paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        [HttpGet("all-payment")]
        public async Task<IActionResult> GetAllPayment()
        {
            var payments = await _paymentRepo.GetAllPayment();
            if(payments == null || !payments.Any()) return BadRequest("Can not find any payment");

            var paymentsDto = payments.Select(p => new PaymentDTO
            {
                PaymentID = p.PaymentID,
                Qrcode = p.Qrcode,
                Type = p.Type
            }).ToList();

            return Ok(paymentsDto);
        }
    }
}
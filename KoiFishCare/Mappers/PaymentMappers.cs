using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Payment;
using KoiFishCare.DTOs.Payment;
using KoiFishCare.Models;

namespace KoiFishCare.Mappers
{
    public static class PaymentMappers
    {
        public static PaymentDTO ToDTOFromModel(this Payment payment)
        {
            return new PaymentDTO {
                PaymentID = payment.PaymentID,
                Type = payment.Type,
                IsDeleted = payment.IsDeleted,
            };
        }
    }
}
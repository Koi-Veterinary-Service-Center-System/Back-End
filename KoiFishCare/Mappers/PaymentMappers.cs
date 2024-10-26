using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Payment;
using KoiFishCare.Models;

namespace KoiFishCare.Mappers
{
    public static class PaymentMappers
    {
        public static Payment ToModelFromDTO(this AddPaymentDTO addPaymentDTO)
        {
            return new Payment
            {
                Type = addPaymentDTO.Type,
            };
        }
    }
}
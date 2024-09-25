using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.Payment
{
    public class PaymentDto
    {
        public int? PaymentID { get; set; }
        public string? Qrcode { get; set; }
        public string? Type { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.Payment
{
    public class AddPaymentDTO
    {
        public string? Qrcode { get; set; }

        [Required(ErrorMessage = "Type of payment is required.")]
        public string Type { get; set; } = null!;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.Booking
{
    public class CancelBookingDto
    {
        public string? BankName { get; set; }
        public string? CustomerBankNumber { get; set; }
        public string? CustomerBankAccountName { get; set; }
        public string Reason { get; set; } = string.Empty;
        
        [Range(0,100)]
        public decimal? RefundPercent { get; set; }
    }
}
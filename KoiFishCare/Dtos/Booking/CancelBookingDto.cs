using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.Booking
{
    public class CancelBookingDto
    {
        public string? BankName { get; set; }
        public string? CustomerBankNumber { get; set; }
        public string? CustomerBankAccountName { get; set; }
    }
}
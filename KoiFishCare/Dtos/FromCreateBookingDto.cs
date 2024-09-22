using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Models.Enum;

namespace KoiFishCare.Dtos
{
    public class FromCreateBookingDto
    {
        public DateOnly? BookingDate { get; set; }
        public string? Location { get; set; }
        public string? Note { get; set; }
        public decimal? TotalAmount { get; set; }
        public BookingStatus BookingStatus { get; set; } = BookingStatus.WaitingForPayment;
    }
}
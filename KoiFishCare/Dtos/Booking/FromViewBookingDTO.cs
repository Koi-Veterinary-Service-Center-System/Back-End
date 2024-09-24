using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.Booking
{
    public class FromViewBookingDTO
    {
        public string CustomerName { get; set; } = string.Empty;

        public string? Note { get; set; }

        public bool KoiOrPoolType { get; set; }

        public string VetName { get; set; } = string.Empty;

        public string? Location { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public string ServiceName { get; set; } = string.Empty;

        public string PaymentType { get; set; } = string.Empty;

        public DateOnly? BookingDate { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Models.Enum;

namespace KoiFishCare.DTOs.Booking
{
    public class FromViewBookingDTO
    {
        public string CustomerName { get; set; } = null!;

        public string? Note { get; set; }

        public bool? KoiOrPoolType { get; set; }

        public string VetName { get; set; } = null!;

        public string? Location { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public string ServiceName { get; set; } = null!;

        public string PaymentType { get; set; } = null!;

        public DateOnly BookingDate { get; set; }

        public BookingStatus BookingStatus { get; set; }
    }
}
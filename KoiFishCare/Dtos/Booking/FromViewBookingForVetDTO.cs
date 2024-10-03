using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Models.Enum;

namespace KoiFishCare.DTOs.Booking
{
    public class FromViewBookingForVetDTO
    {
        public int BookingID { get; set; }

        public DateOnly BookingDate { get; set; }

        public string ServiceName { get; set; } = null!;

        public int SlotID { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public string? CustomerName { get; set; }

        public bool? KoiOrPoolType { get; set; }

        public string? KoiOrPoolName { get; set; }

        public string? Location { get; set; }

        public string PaymentType { get; set; } = null!;

        public decimal? TotalAmount { get; set; }

        public string? Note { get; set; }

        public BookingStatus BookingStatus { get; set; }
    }
}
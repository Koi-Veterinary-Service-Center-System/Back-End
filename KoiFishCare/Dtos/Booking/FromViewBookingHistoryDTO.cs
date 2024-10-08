using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Models;
using KoiFishCare.Models.Enum;

namespace KoiFishCare.Dtos.Booking
{
    public class FromViewBookingHistoryDTO
    {
        public int BookingID { get; set; }

        public DateOnly BookingDate { get; set; }

        public string CustomerName { get; set; } = null!;

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }

        public string ServiceName { get; set; } = null!;

        public string VetName { get; set; } = null!;

        public bool? KoiOrPoolType { get; set; }

        public string? KoiOrPoolName { get; set; }

        public string? Location { get; set; }

        public string PaymentType { get; set; } = null!;

        public string? DiseaseName { get; set; }

        public string? Symptoms { get; set; }

        public string? Medication { get; set; }

        public string? PrescriptionNote { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? RefundPercent { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? RefundMoney { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        public int? Rate { get; set; }

        public string? Comments { get; set; }

        public BookingStatus BookingStatus { get; set; }
    }
}
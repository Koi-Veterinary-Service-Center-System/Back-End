using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Models.Enum;

namespace KoiFishCare.Dtos
{
    public class FromCreateBookingDto
    {
        public string? CustomerUserName { get; set; }
        
        public string? Note { get; set; }

        [Required(ErrorMessage = "KoiOrPoolId is required.")]
        public int KoiOrPoolId { get; set; }

        public string? VetName { get; set; }
        public decimal? TotalAmount { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Slot ID is required.")]
        public int SlotId { get; set; }

        [Required(ErrorMessage = "Service ID is required.")]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Payment ID is required.")]
        public int PaymentId { get; set; }

        [Required(ErrorMessage = "DateOnly is required.")]
        public DateOnly BookingDate { get; set; }
    }
}
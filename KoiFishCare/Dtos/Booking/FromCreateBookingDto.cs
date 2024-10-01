using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Models.Enum;

namespace KoiFishCare.DTOs
{
    public class FromCreateBookingDTO
    {
        public string? Note { get; set; }

        public int? KoiOrPoolId { get; set; }

        [Required(ErrorMessage = "Vet is required.")]
        public string VetName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Total Amount is required.")]
        public decimal TotalAmount { get; set; }

        public string? Location { get; set; }

        [Required(ErrorMessage = "Slot is required.")]

        public int SlotId { get; set; }

        [Required(ErrorMessage = "Service ID is required.")]

        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Payment ID is required.")]

        public int PaymentId { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date)]
        public DateOnly BookingDate { get; set; }
    }
}
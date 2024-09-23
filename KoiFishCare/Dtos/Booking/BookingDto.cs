using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Models.Enum;

namespace KoiFishCare.Dtos
{
    public class BookingDto
    {
        public int? ServiceId { get; set; }
        
        public int? PaymentId { get; set; }
        
        public int? SlotId { get; set; }
        
        public string? CustomerId { get; set; }
        
        public string? VetId { get; set; }
        
        public int? DistanceId { get; set; }
        
        public string? KoiOrPoolId { get; set; }
        
        public DateOnly? BookingDate { get; set; }
        
        public string? Location { get; set; }
        
        public string? Note { get; set; }
        
        public decimal? TotalAmount { get; set; }

        public BookingStatus BookingStatus { get; set; }
    }
}
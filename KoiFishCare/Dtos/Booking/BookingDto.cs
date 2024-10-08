using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Feedback;
using KoiFishCare.Dtos.PrescriptionRecord;

namespace KoiFishCare.Dtos.Booking
{
    public class BookingDTO
    {
        public int? BookingID { get; set; }
        public DateOnly? BookingDate { get; set; }
        public string? Location { get; set; }
        public string? Note { get; set; }
        public decimal TotalAmount { get; set; }
        public string? BookingStatus { get; set; }//de string de chuyen tu enum thanh string
        public string? MeetURL { get; set; }
        public int? PaymentID { get; set; }
        public string? PaymentType { get; set; }
        public int? ServiceID { get; set; }
        public string? ServiceName { get; set; }
        public int? SlotID { get; set; }
        public TimeOnly? SlotStartTime { get; set; }
        public TimeOnly? SlotEndTime { get; set; }
        public string? SlotWeekDate { get; set; } = null!;
        public string? CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public string? VetID { get; set; }
        public string? VetName { get; set; }
        public int? KoiOrPoolID { get; set; }
        public string? KoiOrPoolName { get; set; }
    }

}
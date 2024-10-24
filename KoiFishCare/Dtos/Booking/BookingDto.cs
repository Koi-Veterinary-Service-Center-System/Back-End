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
        public bool? isPaid { get; set; }
        public bool? hasPres { get; set; }
        public DateOnly? BookingDate { get; set; }
        public string? Location { get; set; }
        public string? Note { get; set; }
        public decimal InitAmount { get; set; }
        public string? BookingStatus { get; set; }//de string de chuyen tu enum thanh string
        public string? MeetURL { get; set; }
        public int? PaymentID { get; set; }
        public string? PaymentTypeAtBooking { get; set; }
        public int Quantity { get; set; }
        public int? ServiceID { get; set; }
        public string? ServiceNameAtBooking { get; set; }
        public decimal ServicePriceAtBooking { get; set; }
        public decimal ServiceQuantityPriceAtBooking { get; set; }
        public int? SlotID { get; set; }
        public TimeOnly? SlotStartTimeAtBooking { get; set; }
        public TimeOnly? SlotEndTimeAtBooking { get; set; }
        public string? SlotWeekDateAtBooking { get; set; } = null!;
        public string? CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? VetID { get; set; }
        public string? VetName { get; set; }
        public string? VetEmail { get; set; }
        public string? ImageURL { get; set; }
        public int? BookingRecordID { get; set; }
        public decimal? ArisedMoney { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? BookingRecordNote { get; set; }
    }

}
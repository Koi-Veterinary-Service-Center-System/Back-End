using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.BookingRecord;
using KoiFishCare.Models;

namespace KoiFishCare.Mappers
{
    public static class BookingRecordMappers
    {
        public static BookingRecordDTO ToDTOFromModel(this BookingRecord bookingRecord)
        {
            return new BookingRecordDTO()
            {
                BookingRecordID = bookingRecord.BookingRecordID,
                BookingID = bookingRecord.Booking.BookingID,
                ArisedQuantity = bookingRecord.ArisedQuantity,
                QuantityMoney = bookingRecord.QuantityMoney,
                ReceivableAmount = bookingRecord.Booking.isPaid == true ? bookingRecord.QuantityMoney : bookingRecord.TotalAmount,
                TotalAmount = bookingRecord.TotalAmount,
                Note = bookingRecord.Note,
                RefundMoney = bookingRecord.RefundMoney,
                RefundPercent = bookingRecord.RefundPercent
            };
        }

    }
}
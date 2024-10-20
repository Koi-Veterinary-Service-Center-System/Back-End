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
                ArisedMoney = bookingRecord.ArisedMoney,
                TotalAmount = bookingRecord.TotalAmount,
            };
        }

        public static BookingRecord ToModelFromDTO(this FromCreateBookingRecordDTO fromCreateBookingRecordDTO)
        {
            return new BookingRecord()
            {
                BookingID = fromCreateBookingRecordDTO.BookingID,
                ArisedMoney = fromCreateBookingRecordDTO.ArisedMoney,
                TotalAmount = fromCreateBookingRecordDTO.TotalAmount,
            };
        }

    }
}
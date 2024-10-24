using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Booking;
using KoiFishCare.DTOs;
using KoiFishCare.Models;
using KoiFishCare.Models.Enum;

namespace KoiFishCare.Mappers
{
    public static class BookingMappers
    {
        public static Booking ToBookingFromCreate(this FromCreateBookingDTO createBookingDto, Slot slot, Service service, Payment payment, string customerId, string vetId)
        {
            return new Booking()
            {
                InitAmount = createBookingDto.InitAmount,
                Quantity = createBookingDto.Quantity,
                ServiceID = service.ServiceID,
                PaymentID = createBookingDto.PaymentId,
                SlotID = slot.SlotID,
                BookingDate = createBookingDto.BookingDate,
                Location = createBookingDto.Location,
                Note = createBookingDto.Note,
                BookingStatus = createBookingDto.PaymentId == 1 ? BookingStatus.Scheduled : BookingStatus.Pending,
                CustomerID = customerId,
                VetID = vetId,
                Slot = slot,
                Service = service,
                PaymentTypeAtBooking = payment.Type,
                ServiceNameAtBooking = service.ServiceName,
                ServicePriceAtBooking = service.Price,
                ServiceQuantityPriceAtBooking = service.QuantityPrice,
                SlotStartTimeAtBooking = slot.StartTime,
                SlotEndTimeAtBooking = slot.EndTime,
                SlotWeekDateAtBooking = slot.WeekDate.ToString()
            };
        }

        public static BookingDTO ToDtoFromModel(this Booking booking)
        {
            return new BookingDTO
            {
                BookingID = booking.BookingID,
                isPaid = booking.isPaid,
                hasPres = booking.hasPres,
                BookingDate = booking.BookingDate,
                Location = booking.Location,
                Note = booking.Note,
                InitAmount = booking.InitAmount,
                BookingStatus = booking.BookingStatus.ToString(),
                MeetURL = booking.MeetURL,
                PaymentID = booking.PaymentID,
                PaymentTypeAtBooking = booking.PaymentTypeAtBooking,
                ServiceID = booking.ServiceID,
                ServiceNameAtBooking = booking.ServiceNameAtBooking,
                SlotID = booking.SlotID,
                SlotStartTimeAtBooking = booking.SlotStartTimeAtBooking,
                SlotEndTimeAtBooking = booking.SlotEndTimeAtBooking,
                SlotWeekDateAtBooking = booking.SlotWeekDateAtBooking,
                CustomerID = booking.CustomerID,
                CustomerName = booking.Customer?.UserName,
                PhoneNumber = booking.Customer?.PhoneNumber,
                VetID = booking.VetID,
                VetName = booking.Veterinarian?.UserName,
                VetEmail = booking.Veterinarian?.Email,
                ImageURL = booking.Veterinarian?.ImageURL,
                BookingRecordID = booking.BookingRecord?.BookingRecordID,
                ArisedMoney = booking.BookingRecord?.ArisedMoney,
                TotalAmount = booking.BookingRecord?.TotalAmount,
                BookingRecordNote = booking.BookingRecord?.Note
            };
        }

    }
}
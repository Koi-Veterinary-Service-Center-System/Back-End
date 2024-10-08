using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Booking;
using KoiFishCare.DTOs;
using KoiFishCare.DTOs.Booking;
using KoiFishCare.Models;
using KoiFishCare.Models.Enum;

namespace KoiFishCare.Mappers
{
    public static class BookingMappers
    {
        public static Booking ToBookingFromCreate(this FromCreateBookingDTO createBookingDto)
        {
            return new Booking()
            {
                ServiceID = createBookingDto.ServiceId,
                PaymentID = createBookingDto.PaymentId,
                SlotID = createBookingDto.SlotId,
                KoiOrPoolID = createBookingDto.KoiOrPoolId,
                BookingDate = createBookingDto.BookingDate,
                Location = createBookingDto.Location,
                Note = createBookingDto.Note,
                BookingStatus = BookingStatus.Pending
            };
        }

        public static FromViewBookingDTO ToBookingDtoFromModel(this Booking booking)
        {
            return new FromViewBookingDTO
            {
                BookingDate = booking.BookingDate,
                CustomerName = booking.Customer?.UserName ?? "Unknown",
                StartTime = booking.Slot.StartTime,
                EndTime = booking.Slot.EndTime,
                ServiceName = booking.Service.ServiceName,
                VetName = booking.Veterinarian?.UserName ?? "Unknown",//check null
                KoiOrPoolType = booking.KoiOrPool?.IsPool,//checknull
                KoiOrPoolName = booking.KoiOrPool?.Name,//check null
                Location = booking.Location,
                BookingStatus = booking.BookingStatus,
                Note = booking.Note,
                PaymentType = booking.Payment.Type
            };
        }

        public static BookingDTO ToDtoFromModel(this Booking booking)
        {
            return new BookingDTO
            {
                BookingID = booking.BookingID,
                BookingDate = booking.BookingDate,
                Location = booking.Location,
                Note = booking.Note,
                TotalAmount = booking.TotalAmount,
                BookingStatus = booking.BookingStatus.ToString(),
                MeetURL = booking.MeetURL,
                PaymentID = booking.PaymentID,
                PaymentType = booking.Payment?.Type,
                ServiceID = booking.ServiceID,
                ServiceName = booking.Service?.ServiceName,
                SlotID = booking.SlotID,
                SlotStartTime = booking.Slot?.StartTime,
                SlotEndTime = booking.Slot?.EndTime,
                SlotWeekDate = booking.Slot?.WeekDate,
                CustomerID = booking.CustomerID,
                CustomerName = booking.Customer?.UserName,
                VetID = booking.VetID,
                VetName = booking.Veterinarian?.UserName,
                KoiOrPoolID = booking.KoiOrPoolID,
                KoiOrPoolName = booking.KoiOrPool?.Name
            };
        }

    }
}
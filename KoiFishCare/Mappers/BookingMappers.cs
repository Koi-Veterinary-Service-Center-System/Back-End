using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                BookingStatus = BookingStatus.WaitingForPayment
            };
        }
    }
}
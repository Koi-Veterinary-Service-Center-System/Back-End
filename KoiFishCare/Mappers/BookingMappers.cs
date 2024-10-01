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
                BookingStatus = BookingStatus.Pending
            };
        }

        // public static FromViewBookingDTO ToBookingFromView(this Booking viewBooking)
        // {
        //     return new FromViewBookingDTO()
        //     {
        //        CustomerName = viewBooking.Customer.LastName,
        //        Note = viewBooking.Note,
        //        KoiOrPoolType = viewBooking.KoiOrPool.IsPool,
        //        VetName = viewBooking.Veterinarian.LastName,
        //        Location = viewBooking.Location,
        //        StartTime = viewBooking.Slot.StartTime,
        //        EndTime = viewBooking.Slot.EndTime,
        //        ServiceName =viewBooking.Service.ServiceName,
        //        PaymentType = viewBooking.Payment.Type,
        //        BookingDate = viewBooking.BookingDate,
        //        BookingStatus = viewBooking.BookingStatus,
        //     };
        // }
        
    }
}
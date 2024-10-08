using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Booking;
using KoiFishCare.DTOs.Booking;
using KoiFishCare.Models;
using KoiFishCare.Models.Enum;

namespace KoiFishCare.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> CreateBooking(Booking bookingModel);
        Task<List<FromViewBookingDTO>?> GetBookingsByCusIdAsync(string cusID);
        Task<List<FromViewBookingForVetDTO>?> GetBookingByVetIdAsync(string vetID);
        Task<Booking?> GetBookingByIdAsync(int bookingID);
        Task<List<FromViewBookingHistoryDTO>?> GetBookingByStatusAsync(string userID);
        void UpdateBooking(Booking booking);

        Task<List<BookingDTO>> GetAllBooking();
    }
}
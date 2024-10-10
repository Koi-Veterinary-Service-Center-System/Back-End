using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Booking;
using KoiFishCare.Models;
using KoiFishCare.Models.Enum;

namespace KoiFishCare.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> CreateBooking(Booking bookingModel);
        Task<List<Booking>?> GetBookingsByCusIdAsync(string cusID);
        Task<List<Booking>?> GetBookingByVetIdAsync(string vetID);
        Task<Booking?> GetBookingByIdAsync(int bookingID);
        Task<List<Booking>?> GetBookingByStatusAsync(string userID);
        void UpdateBooking(Booking booking);

        Task<List<BookingDTO>> GetAllBooking();
    }
}
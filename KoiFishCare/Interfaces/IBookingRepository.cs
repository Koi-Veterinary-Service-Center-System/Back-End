using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.DTOs.Booking;
using KoiFishCare.Models;
using KoiFishCare.Models.Enum;

namespace KoiFishCare.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking> CreateBooking(Booking bookingModel);
        Task<List<Booking>> GetBookingsByDateAndSlot(DateOnly date, int slotId);
        Task<List<FromViewBookingDTO>?> GetBookingsByCusIdAsync(string cusID);
        Task<List<FromViewBookingForVetDTO>?> GetBookingByVetIdAsync(string vetID);
        Task<Booking?> GetBookingByIdAsync(int bookingID);
        void UpdateBooking(Booking booking);
    }
}
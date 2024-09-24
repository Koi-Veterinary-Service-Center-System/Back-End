using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Data;
using KoiFishCare.Interfaces;
using KoiFishCare.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiFishCare.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly KoiFishVeterinaryServiceContext _context;
        public BookingRepository(KoiFishVeterinaryServiceContext context)
        {
            _context = context;
        }

        public async Task<Booking> CreateBooking(Booking bookingModel)
        {
            await _context.Bookings.AddAsync(bookingModel);
            await _context.SaveChangesAsync();
            return bookingModel;
        }

        public async Task<List<Booking>> GetBookingsByDateAndSlot(DateOnly date, int slotId)
        {
            return await _context.Bookings.Where(b => b.BookingDate == date && b.SlotID == slotId).ToListAsync();
        }
    }
}
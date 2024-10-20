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
    public class BookingRecordRepository : IBookingRecordRepository
    {
        private readonly KoiFishVeterinaryServiceContext _context;
        public BookingRecordRepository(KoiFishVeterinaryServiceContext context)
        {
            _context = context;
        }

        public async Task<BookingRecord> CreateAsync(BookingRecord bookingRecord)
        {
            await _context.BookingRecords.AddAsync(bookingRecord);
            await _context.SaveChangesAsync();
            return bookingRecord;
        }

        public async Task DeleteAsync(BookingRecord bookingRecord)
        {
            _context.BookingRecords.Remove(bookingRecord);
            await _context.SaveChangesAsync();
        }

        public async Task<List<BookingRecord>?> GetAllAsync()
        {
            return await _context.BookingRecords.Include(x => x.Booking).ToListAsync();
        }

        public async Task<BookingRecord?> GetByBookingIDAsync(int bookingID)
        {
            return await _context.BookingRecords.Include(x => x.Booking).FirstOrDefaultAsync(x => x.Booking.BookingID == bookingID);
        }

        public async Task<BookingRecord?> GetByIDAsync(int bookingRecordID)
        {
            return await _context.BookingRecords.FirstOrDefaultAsync(x => x.BookingRecordID == bookingRecordID);
        }

        public async Task<BookingRecord> UpdateAsync(BookingRecord bookingRecord)
        {
            _context.BookingRecords.Update(bookingRecord);
            await _context.SaveChangesAsync();
            return bookingRecord;
        }
    }
}
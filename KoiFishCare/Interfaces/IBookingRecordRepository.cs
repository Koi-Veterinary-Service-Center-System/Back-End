using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Models;

namespace KoiFishCare.Interfaces
{
    public interface IBookingRecordRepository
    {
        Task<BookingRecord?> GetByBookingIDAsync(int bookingID);

        Task<List<BookingRecord>?> GetAllAsync();

        Task<BookingRecord?> GetByIDAsync(int bookingRecordID);

        Task<BookingRecord> CreateAsync(BookingRecord bookingRecord);

        Task<BookingRecord> UpdateAsync(BookingRecord bookingRecord);

        Task DeleteAsync(BookingRecord bookingRecord);

    }
}
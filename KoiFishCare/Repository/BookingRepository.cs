using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Data;
using KoiFishCare.Dtos.Booking;
using KoiFishCare.DTOs.Booking;
using KoiFishCare.Interfaces;
using KoiFishCare.Mappers;
using KoiFishCare.Migrations;
using KoiFishCare.Models;
using KoiFishCare.Models.Enum;
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

            //load relate entities
            await _context.Entry(bookingModel)
            .Reference(b => b.Customer).LoadAsync();

            await _context.Entry(bookingModel)
            .Reference(b => b.Veterinarian).LoadAsync();

            await _context.Entry(bookingModel)
            .Reference(b => b.Slot).LoadAsync();

            await _context.Entry(bookingModel)
            .Reference(b => b.Service).LoadAsync();

            await _context.Entry(bookingModel)
            .Reference(b => b.KoiOrPool).LoadAsync();

            await _context.Entry(bookingModel)
            .Reference(b => b.Payment).LoadAsync();

            return bookingModel;
        }




        public async Task<Booking?> GetBookingByIdAsync(int bookingID)
        {
            var booking = await _context.Bookings.FindAsync(bookingID);
            if (booking == null)
            {
                return null;
            }
            return booking;

        }


        void IBookingRepository.UpdateBooking(Booking booking)
        {
            _context.Bookings.Update(booking);
            _context.SaveChanges();
        }



        public async Task<List<BookingDTO>> GetAllBooking()
        {
            return await _context.Bookings
                .Include(b => b.Payment)
                .Include(b => b.Service)
                .Include(b => b.Slot)
                .Include(b => b.Customer)
                .Include(b => b.Veterinarian)
                .Include(b => b.KoiOrPool)
                .Select(b => b.ToDtoFromModel())
                .ToListAsync();
        }

        public async Task<List<Booking>?> GetBookingsByCusIdAsync(string cusID)
        {
            var bookings = await _context.Bookings
                .Include(b => b.Service)
                .Include(b => b.Slot)
                .Include(b => b.Customer)
                .Include(b => b.Veterinarian)
                .Include(b => b.KoiOrPool)
                .Include(b => b.Payment)
                .Where(b => b.CustomerID == cusID &&
                    (b.BookingStatus == BookingStatus.Pending ||
                        b.BookingStatus == BookingStatus.Confirmed ||
                        b.BookingStatus == BookingStatus.Scheduled ||
                        b.BookingStatus == BookingStatus.Ongoing ||
                        b.BookingStatus == BookingStatus.Completed ||
                        b.BookingStatus == BookingStatus.Received_Money
                    )).ToListAsync();

            if (bookings == null)
            {
                return null;
            }

            return bookings.ToList();
        }

        public async Task<List<Booking>?> GetBookingByVetIdAsync(string vetID)
        {
            var bookings = await _context.Bookings
                .Include(b => b.Payment)
                .Include(b => b.Service)
                .Include(b => b.Slot)
                .Include(b => b.Customer)
                .Include(b => b.Veterinarian)
                .Include(b => b.KoiOrPool)
                .Where(b => b.VetID == vetID &&
                    (b.BookingStatus == BookingStatus.Scheduled ||
                        b.BookingStatus == BookingStatus.Ongoing ||
                        b.BookingStatus == BookingStatus.Completed ||
                        b.BookingStatus == BookingStatus.Received_Money
                    )).ToListAsync();

            if (bookings == null)
            {
                return null;
            }

            return bookings.ToList();
        }

        public async Task<List<Booking>?> GetBookingByStatusAsync(string userID)
        {
            var bookings = await _context.Bookings
                .Include(b => b.Payment)
                .Include(b => b.Service)
                .Include(b => b.Slot)
                .Include(b => b.Customer)
                .Include(b => b.Veterinarian)
                .Include(b => b.KoiOrPool)
                .Where(x => x.Customer.Id.Equals(userID) &&
                    (x.BookingStatus == BookingStatus.Succeeded ||
                        x.BookingStatus == BookingStatus.Cancelled
                      )).ToListAsync();

            if (bookings == null)
            {
                return null;
            }

            return bookings.ToList();
        }

    }
}

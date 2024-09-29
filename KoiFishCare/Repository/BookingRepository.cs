using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Data;
using KoiFishCare.DTOs.Booking;
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

        public async Task<List<FromViewBookingForVetDTO>?> GetBookingByVetIdAsync(string vetID)
        {
            var booking = await _context.Bookings.
            Include(u => u.Customer).
            Where(b => b.VetID == vetID).
            Select(b => new FromViewBookingForVetDTO
            {
                BookingDate = b.BookingDate,
                ServiceName = b.Service.ServiceName,
                SlotID = b.Slot.SlotID,
                StartTime = b.Slot.StartTime,
                EndTime = b.Slot.EndTime,
                CustomerName = b.Customer.LastName,
                KoiOrPoolType = b.KoiOrPool != null ? b.KoiOrPool.IsPool : true,
                Location = b.Location,
                PaymentType = b.Payment.Type,
                TotalAmount = b.TotalAmount,
                Note = b.Note,
                BookingStatus = b.BookingStatus,
            }).ToListAsync();

            if (booking == null)
            {
                return null;
            }

            return booking.ToList();
        }

        public async Task<List<Booking>> GetBookingsByDateAndSlot(DateOnly date, int slotId)
        {
            return await _context.Bookings.Where(b => b.BookingDate == date && b.SlotID == slotId).ToListAsync();
        }

        public async Task<List<FromViewBookingDTO>?> GetBookingsByCusIdAsync(string cusID)
        {
            var booking = await _context.Bookings.
                        Include(u => u.Customer).
                        Include(s => s.Service).
                        Include(sl => sl.Slot).
                        Include(v => v.Veterinarian).
                        Include(kob => kob.KoiOrPool).
                        Include(p => p.Payment).
                        Where(b => b.CustomerID == cusID).
                        Select(b => new FromViewBookingDTO
                        {
                            CustomerName = b.Customer.LastName,
                            Location = b.Location,
                            ServiceName = b.Service.ServiceName,
                            KoiOrPoolType = b.KoiOrPool != null ? b.KoiOrPool.IsPool : true,
                            StartTime = b.Slot.StartTime,
                            EndTime = b.Slot.EndTime,
                            VetName = b.Veterinarian.FirstName,
                            Note = b.Note,
                            PaymentType = b.Payment.Type,
                            BookingDate = b.BookingDate,
                            BookingStatus = b.BookingStatus,
                        }).ToListAsync();

            if (booking == null)
            {
                return null;
            }

            return booking.ToList();

        }
    }
}
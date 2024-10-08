using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Data;
using KoiFishCare.Dtos.Booking;
using KoiFishCare.DTOs.Booking;
using KoiFishCare.Interfaces;
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

        public async Task<List<FromViewBookingForVetDTO>?> GetBookingByVetIdAsync(string vetID)
        {
            var booking = await _context.Bookings.
            Where(b => b.VetID == vetID && (b.BookingStatus == BookingStatus.Scheduled || b.BookingStatus == BookingStatus.Ongoing || b.BookingStatus == BookingStatus.Completed || b.BookingStatus == BookingStatus.Received_Money)).
            Select(b => new FromViewBookingForVetDTO
            {
                BookingID = b.BookingID,
                BookingDate = b.BookingDate,
                ServiceName = b.Service.ServiceName,
                SlotID = b.Slot.SlotID,
                StartTime = b.Slot.StartTime,
                EndTime = b.Slot.EndTime,
                CustomerName = b.Customer.FirstName + " " + b.Customer.LastName,
                KoiOrPoolType = b.KoiOrPool != null ? b.KoiOrPool.IsPool : (bool?)null,
                KoiOrPoolName = b.KoiOrPool != null ? b.KoiOrPool.Name : null,
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

        public async Task<List<FromViewBookingDTO>?> GetBookingsByCusIdAsync(string cusID)
        {
            var booking = await _context.Bookings.
                        Where(b => b.CustomerID == cusID && (b.BookingStatus == BookingStatus.Pending || b.BookingStatus == BookingStatus.Confirmed || b.BookingStatus == BookingStatus.Scheduled || b.BookingStatus == BookingStatus.Ongoing || b.BookingStatus == BookingStatus.Completed || b.BookingStatus == BookingStatus.Received_Money)).
                        Select(b => new FromViewBookingDTO
                        {
                            BookingID = b.BookingID,
                            CustomerName = b.Customer.FirstName + " " + b.Customer.LastName,
                            Location = b.Location,
                            ServiceName = b.Service.ServiceName,
                            KoiOrPoolType = b.KoiOrPool != null ? b.KoiOrPool.IsPool : (bool?)null,
                            KoiOrPoolName = b.KoiOrPool != null ? b.KoiOrPool.Name : null,
                            StartTime = b.Slot.StartTime,
                            EndTime = b.Slot.EndTime,
                            VetName = b.Veterinarian.FirstName + " " + b.Veterinarian.LastName,
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

        public async Task<List<FromViewBookingHistoryDTO>?> GetBookingByStatusAsync(string userID)
        {
            var bookings = await _context.Bookings.
            Include(c => c.Customer).
            Where(x => x.Customer.Id.Equals(userID) && (x.BookingStatus == BookingStatus.Succeeded || x.BookingStatus == BookingStatus.Cancelled)).
            Select(b => new FromViewBookingHistoryDTO
            {
                BookingID = b.BookingID,
                CustomerName = b.Customer.FirstName + " " + b.Customer.LastName,
                KoiOrPoolType = b.KoiOrPool != null ? b.KoiOrPool.IsPool : (bool?)null,
                KoiOrPoolName = b.KoiOrPool != null ? b.KoiOrPool.Name : null,
                VetName = b.Veterinarian.FirstName + " " + b.Veterinarian.LastName,
                Location = b.Location,
                StartTime = b.Slot.StartTime,
                EndTime = b.Slot.EndTime,
                ServiceName = b.Service.ServiceName,
                PaymentType = b.Payment.Type,
                DiseaseName = _context.PrescriptionRecords
                .Where(pr => pr.BookingID == b.BookingID)
                .Select(pr => pr.DiseaseName)
                .FirstOrDefault(),
                Symptoms = _context.PrescriptionRecords
                .Where(pr => pr.BookingID == b.BookingID)
                .Select(pr => pr.Symptoms)
                .FirstOrDefault(),
                Medication = _context.PrescriptionRecords
                .Where(pr => pr.BookingID == b.BookingID)
                .Select(pr => pr.Medication)
                .FirstOrDefault(),
                PrescriptionNote = _context.PrescriptionRecords
                .Where(pr => pr.BookingID == b.BookingID)
                .Select(pr => pr.Note)
                .FirstOrDefault(),
                RefundPercent = _context.PrescriptionRecords
                .Where(pr => pr.BookingID == b.BookingID)
                .Select(pr => pr.RefundPercent)
                .FirstOrDefault(),
                RefundMoney = _context.PrescriptionRecords
                .Where(pr => pr.BookingID == b.BookingID)
                .Select(pr => pr.RefundMoney)
                .FirstOrDefault(),
                BookingDate = b.BookingDate,
                TotalAmount = b.TotalAmount,
                Rate = _context.Feedbacks
                .Where(f => f.BookingID == b.BookingID)
                .Select(f => f.Rate).FirstOrDefault(),
                Comments = _context.Feedbacks
                .Where(f => f.BookingID == b.BookingID)
                .Select(f => f.Comments).FirstOrDefault(),
                BookingStatus = b.BookingStatus,
            }).ToListAsync();

            return bookings;

        }
    }
}
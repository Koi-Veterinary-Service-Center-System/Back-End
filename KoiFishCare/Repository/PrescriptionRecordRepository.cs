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
    public class PrescriptionRecordRepository : IPrescriptionRecordRepository
    {
        private readonly KoiFishVeterinaryServiceContext _context;
        public PrescriptionRecordRepository(KoiFishVeterinaryServiceContext context)
        {
            _context = context;
        }
        public async Task<PrescriptionRecord?> Create(PrescriptionRecord model)
        {
            var bookingExist = await _context.Bookings.AnyAsync(b => b.BookingID == model.BookingID);
            if (!bookingExist) return null;
            await _context.PrescriptionRecords.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<PrescriptionRecord?> Delete(int id)
        {
            var model = await _context.PrescriptionRecords.FirstOrDefaultAsync(p => p.PrescriptionRecordID == id);
            if (model == null) return null;
            _context.Remove(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<PrescriptionRecord?> GetById(int id)
        {
            var model = await _context.PrescriptionRecords.FirstOrDefaultAsync(p => p.BookingID == id);
            return model;
        }

        public async Task<List<PrescriptionRecord>> GetListByCusUsername(string cusName)
        {
            var list = await _context.PrescriptionRecords.Where(p => p.Booking.Customer.UserName.ToLower().Trim() == cusName.ToLower().Trim()).ToListAsync();
            return list;
        }

        public async Task<PrescriptionRecord?> Update(int id, PrescriptionRecord model)
        {
            var exist = await _context.PrescriptionRecords.FirstOrDefaultAsync(p => p.PrescriptionRecordID == id);
            if (exist == null) return null;

            exist.DiseaseName = model.DiseaseName;
            exist.Symptoms = model.Symptoms;
            exist.Medication = model.Medication;
            exist.Note = model.Note;
            await _context.SaveChangesAsync();
            return exist;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Interfaces;
using Microsoft.EntityFrameworkCore;
using KoiFishCare.Data;
using KoiFishCare.Models;

namespace KoiFishCare.Repository
{
    public class SlotRepository : ISlotRepository
    {
        private readonly KoiFishVeterinaryServiceContext _context;
        public SlotRepository(KoiFishVeterinaryServiceContext context)
        {
            _context = context;
        }

        public async Task<Slot> Create(Slot model)
        {
            await _context.Slots.AddAsync(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<Slot?> Delete(int id)
        {
            var result = await _context.Slots.FirstOrDefaultAsync(s => s.SlotID == id);
            if(result == null) return null;

            _context.Slots.Remove(result);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<List<Slot>> GetAllSlot()
        {
            return await _context.Slots.ToListAsync();
        }

        public async Task<VetSlot?> GetAvailableVet(Slot slot)
        {
            // Find VetSlots that match the given slot and have no bookings
            return await _context.VetSlots
                .Include(vs => vs.Veterinarian) // Include Veterinarian details if needed
                .Where(vs => vs.SlotID == slot.SlotID && vs.isBooked == false) // Check if there are no bookings for the slot
                .FirstOrDefaultAsync();
        }

        public async Task<List<VetSlot?>> GetListAvailableSlot(string vetId)
        {
            return await _context.VetSlots.Include(vs => vs.Slot)
                .Where(vs => vs.VetID == vetId && vs.isBooked == false)
                .ToListAsync();
        }

        public async Task<List<VetSlot?>> GetListAvailableVet(int slotId)
        {
            return await _context.VetSlots.Include(vs => vs.Veterinarian)
                .Where(vs => vs.SlotID == slotId && vs.isBooked == false)
                .ToListAsync();
        }

        public async Task<Slot?> GetSlotById(int id)
        {
            return await _context.Slots.FirstOrDefaultAsync(s => s.SlotID == id);
        }

        public async Task<Slot?> Update(int id, Slot model)
        {
            var result = await _context.Slots.FirstOrDefaultAsync(s => s.SlotID == id);
            if(result == null) return null;

            result.StartTime = model.StartTime;
            result.EndTime = model.EndTime;
            result.WeekDate = model.WeekDate;

            await _context.SaveChangesAsync();
            return result;
        }
    }
}
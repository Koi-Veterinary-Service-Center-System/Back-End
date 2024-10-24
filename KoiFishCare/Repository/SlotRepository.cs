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
            if (result == null) return null;

            _context.Slots.Remove(result);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<List<Slot>> GetAllSlot()
        {
            return await _context.Slots.Where(s => s.isDeleted == false).ToListAsync();
        }

        public async Task<VetSlot?> GetAvailableVet(Slot slot)
        {
            // Find VetSlots that match the given slot and have no bookings
            // return await _context.VetSlots
            //     .Include(vs => vs.Veterinarian) // Include Veterinarian details if needed
            //     .Where(vs => vs.SlotID == slot.SlotID && vs.isBooked == false) // Check if there are no bookings for the slot
            //     .FirstOrDefaultAsync();

            // Find VetSlots that match the given slot and have no bookings
            var availableVets = await _context.VetSlots
                .Include(vs => vs.Veterinarian) // Include Veterinarian details if needed
                .Where(vs => vs.SlotID == slot.SlotID && vs.isBooked == false) // Check if there are no bookings for the slot
                .ToListAsync(); // Get all available vet slots as a list

            // Check if there are any available vets
            if (availableVets.Count == 0)
            {
                return null; // No available vets found
            }

            // Select a random available vet slot
            var random = new Random();
            var randomIndex = random.Next(availableVets.Count);
            return availableVets[randomIndex]; // Return a random available vet slot

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
            return await _context.Slots.FirstOrDefaultAsync(s => s.SlotID == id && s.isDeleted == false);
        }

        public async Task Update(Slot slot)
        {
            _context.Slots.Update(slot);
            await _context.SaveChangesAsync();
        }

        public async Task<Slot?> UpdateSlot(int id, Slot model)
        {
            var result = await _context.Slots.FirstOrDefaultAsync(s => s.SlotID == id);
            if (result == null) return null;

            result.StartTime = model.StartTime;
            result.EndTime = model.EndTime;
            result.WeekDate = model.WeekDate;

            await _context.SaveChangesAsync();
            return result;
        }
    }
}
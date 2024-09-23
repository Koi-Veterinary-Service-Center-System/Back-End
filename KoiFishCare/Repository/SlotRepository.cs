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

        public async Task<VetSlot?> GetAvailableVet(Slot slot)
        {
            // Find VetSlots that match the given slot and have no bookings
            return await _context.VetSlots
                .Include(vs => vs.Veterinarian) // Include Veterinarian details if needed
                .Where(vs => vs.SlotID == slot.SlotID && !vs.Slot.Bookings.Any()) // Check if there are no bookings for the slot
                .FirstOrDefaultAsync();
        }


        public async Task<Slot?> GetSlotById(int id)
        {
            return await _context.Slots.FirstOrDefaultAsync(s => s.SlotID == id);
        }
    }
}
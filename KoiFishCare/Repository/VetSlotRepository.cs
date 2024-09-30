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
    public class VetSlotRepository : IVetSlotRepository
    {
        private readonly KoiFishVeterinaryServiceContext _context;
        public VetSlotRepository(KoiFishVeterinaryServiceContext context)
        {
            _context = context;
        }
        public async Task<VetSlot> Create(string vetId, int slotId, bool isBooked)
        {
            var vetSlot = new VetSlot
            {
                VetID = vetId,
                SlotID = slotId,
                isBooked = isBooked
            };

            await _context.VetSlots.AddAsync(vetSlot);
            await _context.SaveChangesAsync();

            // Load the related Slot and Veterinarian entities
            await _context.Entry(vetSlot)
                .Reference(vs => vs.Slot)
                .LoadAsync();

            await _context.Entry(vetSlot)
                .Reference(vs => vs.Veterinarian)
                .LoadAsync();

            return vetSlot;
        }


        public async Task<VetSlot?> Delete(string vetId, int slotId)
        {
            var vetSlot = await _context.VetSlots.FirstOrDefaultAsync(vs => vs.VetID == vetId && vs.SlotID == slotId);
            if (vetSlot == null) return null;

            _context.VetSlots.Remove(vetSlot);
            await _context.SaveChangesAsync();
            return vetSlot;

        }

        public async Task<List<VetSlot>> GetAllVetSlot()
        {
            return await _context.VetSlots.Include(vs => vs.Veterinarian)
            .Include(vs => vs.Slot).ToListAsync();
        }

        public async Task<VetSlot?> GetVetSlot(string vetId, int slotId)
        {
            return await _context.VetSlots.Include(vs => vs.Veterinarian)
            .Include(vs => vs.Slot).FirstOrDefaultAsync(vs => vs.VetID == vetId && vs.SlotID == slotId);
        }

        public async Task<VetSlot?> Update(string vetId, int slotId, bool isBooked)
        {
            var vetSlot = await _context.VetSlots.FirstOrDefaultAsync(vs => vs.VetID == vetId && vs.SlotID == slotId);
            if (vetSlot == null) return null;

            vetSlot.isBooked = isBooked;
            await _context.SaveChangesAsync();
            
            // Load the related Slot and Veterinarian entities
            await _context.Entry(vetSlot)
                .Reference(vs => vs.Slot)
                .LoadAsync();

            await _context.Entry(vetSlot)
                .Reference(vs => vs.Veterinarian)
                .LoadAsync();

            return vetSlot;
        }
    }
}
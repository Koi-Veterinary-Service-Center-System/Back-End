using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using KoiFishCare.Data;
using KoiFishCare.Interfaces;
using KoiFishCare.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiFishCare.Repository
{
    public class VetRepository : IVetRepository
    {
        private readonly KoiFishVeterinaryServiceContext _context;
        public VetRepository(KoiFishVeterinaryServiceContext context)
        {
            _context = context;
        }
        public async Task<List<Veterinarian>> GetAllVet()
        {
            return await _context.Veterinarians.ToListAsync();
        }

        public async Task<Veterinarian?> GetVetById(string id)
        {
            return await _context.Veterinarians.FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task SaveVetAsync(Veterinarian veterinarian)
        {
            await _context.Veterinarians.AddAsync(veterinarian);
            await _context.SaveChangesAsync();
        }
    }
}
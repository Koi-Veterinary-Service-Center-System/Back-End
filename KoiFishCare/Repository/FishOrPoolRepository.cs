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
    public class FishOrPoolRepository : IFishOrPoolRepository
    {
        private readonly KoiFishVeterinaryServiceContext _context;
        public FishOrPoolRepository(KoiFishVeterinaryServiceContext context)
        {
            _context = context;   
        }
        public async Task<KoiOrPool?> GetKoiOrPoolById(int id)
        {
            return await _context.KoiOrPools.FirstOrDefaultAsync(k => k.KoiOrPoolID == id);
        }

        public async Task<List<KoiOrPool>> GetKoiOrPoolsOfCustomer(string customerId)
        {
            return await _context.KoiOrPools.Where(k => k.CustomerId == customerId).ToListAsync();
        }
    }
}
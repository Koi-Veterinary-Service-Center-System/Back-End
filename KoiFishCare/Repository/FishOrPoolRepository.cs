using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Interfaces;
using Microsoft.EntityFrameworkCore;
using KoiFishCare.Data;
using KoiFishCare.Models;
using Microsoft.AspNetCore.Mvc;

namespace KoiFishCare.Repository
{
    public class FishOrPoolRepository : IFishOrPoolRepository
    {
        private readonly KoiFishVeterinaryServiceContext _context;
        public FishOrPoolRepository(KoiFishVeterinaryServiceContext context)
        {
            _context = context;   
        }

        public async Task<KoiOrPool> Create(KoiOrPool koiOrPoolModel)
        {
            await _context.KoiOrPools.AddAsync(koiOrPoolModel);
            await _context.SaveChangesAsync();
            return koiOrPoolModel;
        }

        public async Task<KoiOrPool?> Delete(int id)
        {
            var result = await _context.KoiOrPools.FirstOrDefaultAsync(k => k.KoiOrPoolID == id);
            if(result == null) return null;

            _context.KoiOrPools.Remove(result);
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<KoiOrPool?> GetKoiOrPoolById(int id)
        {
            return await _context.KoiOrPools.FirstOrDefaultAsync(k => k.KoiOrPoolID == id);
        }

        public async Task<List<KoiOrPool>> GetKoiOrPoolsOfCustomer(string customerId)
        {
            return await _context.KoiOrPools.Where(k => k.CustomerID == customerId).ToListAsync();
        }

        public async Task<KoiOrPool?> Update(int id, KoiOrPool koiOrPoolModel)
        {
            var result = await _context.KoiOrPools.FirstOrDefaultAsync(k => k.KoiOrPoolID == id);
            if(result == null) return null;

            result.Name = koiOrPoolModel.Name;
            result.IsPool = koiOrPoolModel.IsPool;
            result.Description = koiOrPoolModel.Description;

            await _context.SaveChangesAsync();
            return result;
        }
    }
}
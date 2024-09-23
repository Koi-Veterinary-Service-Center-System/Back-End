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
    public class ServiceRepository : IServiceRepository
    {
        private readonly KoiFishVeterinaryServiceContext _context;
        public ServiceRepository(KoiFishVeterinaryServiceContext context)
        {
            _context = context;
        }
        public async Task<Service?> GetServiceById(int id)
        {
            return await _context.Services.FirstOrDefaultAsync(s => s.ServiceID == id);
        }
    }
}
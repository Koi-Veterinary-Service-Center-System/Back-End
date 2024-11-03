using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Interfaces;
using Microsoft.EntityFrameworkCore;
using KoiFishCare.Data;
using KoiFishCare.Models;
using KoiFishCare.DTOs.Service;

namespace KoiFishCare.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly KoiFishVeterinaryServiceContext _context;
        public ServiceRepository(KoiFishVeterinaryServiceContext context)
        {
            _context = context;
        }

        public async Task<Service> CreateService(Service service)
        {
            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
            return service;
        }

        public async Task<Service?> DeleteService(int id)
        {
            var service = await _context.Services.FirstOrDefaultAsync(s => s.ServiceID == id);

            if (service == null) return null;

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return service;
        }

        public async Task<List<Service>> GetAllService()
        {
            return await _context.Services.Where(s => s.IsDeleted == false).ToListAsync();
        }

        public async Task<Service?> GetServiceById(int id)
        {
            return await _context.Services.FirstOrDefaultAsync(s => s.ServiceID == id && s.IsDeleted == false);
        }

        public async Task Update(Service service)
        {
            _context.Services.Update(service);
            await _context.SaveChangesAsync();
        }

        public async Task<Service?> UpdateService(int id, AddUpdateServiceDTO updateDto)
        {
            var service = await _context.Services.FirstOrDefaultAsync(s => s.ServiceID == id);

            if (service == null) return null;

            service.ServiceName = updateDto.ServiceName;
            service.Description = updateDto.Description;
            service.Price = updateDto.Price;
            service.EstimatedDuration = updateDto.EstimatedDuration;
            service.QuantityPrice = updateDto.QuantityPrice;
            service.ImageURL = updateDto.ImageURL;
            service.IsAtHome = updateDto.IsAtHome;

            await _context.SaveChangesAsync();
            return service;
        }
    }
}
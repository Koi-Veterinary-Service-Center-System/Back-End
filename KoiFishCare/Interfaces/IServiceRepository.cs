using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Service;
using KoiFishCare.Models;

namespace KoiFishCare.Interfaces
{
    public interface IServiceRepository
    {
        Task<Service?> GetServiceById(int id);
        Task<List<Service>> GetAllService();
        Task<Service> CreateService(Service service);
        Task<Service?> UpdateService(int id, AddUpdateServiceDto updateDto);
        Task<Service?> DeleteService(int id);
    }
}
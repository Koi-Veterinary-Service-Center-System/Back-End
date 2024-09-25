using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Models;

namespace KoiFishCare.Interfaces
{
    public interface IServiceRepository
    {
        Task<Service?> GetServiceById(int id);
        Task<List<Service>> GetAllService();
    }
}
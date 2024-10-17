using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Models;

namespace KoiFishCare.Interfaces
{
    public interface IFishOrPoolRepository
    {
        Task<KoiOrPool?> GetKoiOrPoolById(int id);
        Task<List<KoiOrPool>> GetKoiOrPoolsOfCustomer(string customerId);
        Task<KoiOrPool> Create(KoiOrPool koiOrPoolModel);
        Task<KoiOrPool?> UpdateFishOrPool(int id, KoiOrPool koiOrPoolModel);
        Task<KoiOrPool?> Delete(int id);
        Task Update(KoiOrPool koiOrPool);
    }
}
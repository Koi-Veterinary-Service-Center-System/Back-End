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
    }
}
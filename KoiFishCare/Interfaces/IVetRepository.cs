using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using KoiFishCare.Models;

namespace KoiFishCare.Interfaces
{
    public interface IVetRepository
    {
        Task<List<Veterinarian>> GetAllVet();
        Task<Veterinarian?> GetVetById(string id);
        Task SaveVetAsync(Veterinarian veterinarian);
    }
}
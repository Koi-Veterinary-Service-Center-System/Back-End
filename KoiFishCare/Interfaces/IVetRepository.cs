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
        Task<List<User>?> GetAllVet();
        Task<User?> GetVetById(string id);
        Task SaveVetAsync(User veterinarian);
    }
}
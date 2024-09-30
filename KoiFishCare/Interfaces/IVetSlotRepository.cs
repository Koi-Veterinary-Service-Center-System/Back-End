using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Models;

namespace KoiFishCare.Interfaces
{
    public interface IVetSlotRepository
    {
        Task<List<VetSlot>> GetAllVetSlot();
        Task<VetSlot?> GetVetSlot(string vetId, int slotId);
        Task<VetSlot> Create(string vetId, int slotId, bool isBooked);
        Task<VetSlot?> Update(string vetId, int slotId, bool isBooked);
        Task<VetSlot?> Delete(string vetId, int slotId);
    }
}
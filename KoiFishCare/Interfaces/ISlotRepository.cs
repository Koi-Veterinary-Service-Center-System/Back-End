using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Models;

namespace KoiFishCare.Interfaces
{
    public interface ISlotRepository
    {
        Task<Slot?> GetSlotById(int id);
        Task<VetSlot?> GetAvailableVet(Slot slot);
        Task<List<Slot>> GetAllSlot();
        Task<List<VetSlot?>> GetListAvailableSlot(string vetId);
        Task<List<VetSlot?>> GetListAvailableVet(int slotId);
        Task<Slot> Create(Slot model);
        Task<Slot?> Update(int id, Slot model);
        Task<Slot?> Delete(int id);
    }
}
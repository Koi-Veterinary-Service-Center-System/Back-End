using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Models;

namespace KoiFishCare.Interfaces
{
    public interface IPrescriptionRecordRepository
    {
        Task<List<PrescriptionRecord>> GetListById(int id);
        Task<PrescriptionRecord?> GetById(int id);
        Task<List<PrescriptionRecord>> GetListByCusUsername(string cusName);
        Task<PrescriptionRecord?> Create(PrescriptionRecord model);
        Task<PrescriptionRecord?> Update(int id, PrescriptionRecord model);
        Task<PrescriptionRecord?> Delete(int id);
    }
}
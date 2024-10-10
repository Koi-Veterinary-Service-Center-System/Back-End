using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Models;

namespace KoiFishCare.Interfaces
{
    public interface IPaymentRepository
    {
        Task<List<Payment>> GetAllPayment();
        Task<Payment?> GetPaymentByID(int id);
        Task Add(Payment payment);
        Task Update(Payment payment);
        Task Delete(Payment payment);
    }
}
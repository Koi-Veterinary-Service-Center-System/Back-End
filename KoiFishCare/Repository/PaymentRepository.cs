using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Data;
using KoiFishCare.Interfaces;
using KoiFishCare.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiFishCare.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly KoiFishVeterinaryServiceContext _context;
        public PaymentRepository(KoiFishVeterinaryServiceContext context)
        {
            _context = context;
        }

        public async Task Add(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Payment payment)
        {
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Payment>> GetAllPayment()
        {
            return await _context.Payments.ToListAsync();
        }

        public async Task<Payment?> GetPaymentByID(int id)
        {
            return await _context.Payments.FirstOrDefaultAsync(x => x.PaymentID == id);
        }

        public async Task Update(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
        }
    }
}
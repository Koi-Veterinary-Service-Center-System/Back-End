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
        public async Task<List<Payment>> GetAllPayment()
        {
            return await _context.Payments.ToListAsync();
        }
    }
}
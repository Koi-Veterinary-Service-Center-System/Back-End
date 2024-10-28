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
    public class StaffRepository : IStaffRepository
    {
        private readonly KoiFishVeterinaryServiceContext _context;
        public StaffRepository(KoiFishVeterinaryServiceContext context)
        {
            _context = context;
        }

        public async Task SaveStaffAsync(User staff)
        {
            await _context.Users.AddAsync(staff);
            await _context.SaveChangesAsync();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using KoiFishCare.Data;
using KoiFishCare.Interfaces;
using KoiFishCare.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiFishCare.Repository
{
    public class VetRepository : IVetRepository
    {
        private readonly KoiFishVeterinaryServiceContext _context;
        public VetRepository(KoiFishVeterinaryServiceContext context)
        {
            _context = context;
        }
        public async Task<List<User>?> GetAllVet()
        {
            var vetRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Vet");
            if (vetRole == null)
            {
                return null;
            }
            return await _context.Users.Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == vetRole.Id)).ToListAsync();
        }

        public async Task<User?> GetVetById(string id)
        {
            var vetRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Vet");
            if (vetRole == null)
            {
                return null;
            }
            return await _context.Users.Where(u => _context.UserRoles.Any(ur => ur.UserId == id && ur.RoleId == vetRole.Id)).FirstOrDefaultAsync();
        }

        public async Task SaveVetAsync(User veterinarian)
        {
            await _context.Users.AddAsync(veterinarian);
            await _context.SaveChangesAsync();
        }
    }
}
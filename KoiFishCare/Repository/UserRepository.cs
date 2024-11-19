using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Data;
using KoiFishCare.Dtos.User;
using KoiFishCare.DTOs;
using KoiFishCare.DTOs.User;
using KoiFishCare.Interfaces;
using KoiFishCare.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace KoiFishCare.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly KoiFishVeterinaryServiceContext _context;
        public UserRepository(KoiFishVeterinaryServiceContext context)
        {
            _context = context;
        }

        public async Task UpdateAsync(User user)
        {
            var updateUser = _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>?> GetAllUserAsync()
        {
            var managerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Manager");
            if (managerRole == null)
            {
                return null;
            }
            return await _context.Users.Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId != managerRole.Id)).ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(string userID)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == userID);
        }

        public async Task<User?> UpdateAsync(string userID, UpdateUserProfileDTO userDTO)
        {
            var user = await _context.Users.FindAsync(userID);
            if (user == null)
            {
                return null;
            }

            user.UserName = userDTO.UserName;
            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.Gender = userDTO.Gender;
            user.Email = userDTO.Email;
            user.Address = userDTO.Address;
            user.ImageURL = userDTO.ImageURL;
            user.PhoneNumber = userDTO.PhoneNumber;
            user.ExperienceYears = userDTO.ExperienceYears;

            await _context.SaveChangesAsync();
            return user;
        }
    }
}
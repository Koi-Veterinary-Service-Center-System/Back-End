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

        public async Task<List<User>?> GetAllUserAsync()
        {
            return await _context.Users.ToListAsync();
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Data;
using KoiFishCare.Dtos;
using KoiFishCare.Dtos.User;
using KoiFishCare.Interfaces;
using KoiFishCare.Models;

namespace KoiFishCare.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly KoiFishVeterinaryServiceContext _context;
        public UserRepository(KoiFishVeterinaryServiceContext context)
        {
            _context = context;
        }

        public async Task<User?> UpdateAsync(string UserID, UpdateUserProfileDTO userDTO)
        {
            var user = await _context.Users.FindAsync(UserID);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.DTOs.User;
using KoiFishCare.Models;
using Microsoft.AspNetCore.Identity;

namespace KoiFishCare.Mappers
{
    public static class UserMappers
    {
        public static async Task<UserProfileDTO> ToUserProfileFromUser(this User user, UserManager<User> userManager)
        {
            var role = await userManager.GetRolesAsync(user);
            return new UserProfileDTO
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                Email = user.Email,
                Address = user.Address,
                ImageURL = user.ImageURL,
                ImagePublicId = user.ImagePublicId,
                PhoneNumber = user.PhoneNumber,
                ExperienceYears = user.ExperienceYears,
                Role = role.FirstOrDefault()
            };
        }
    }
}
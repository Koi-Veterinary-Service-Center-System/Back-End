using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.User;
using KoiFishCare.Models;

namespace KoiFishCare.Mappers
{
    public static class UserMappers
    {
        public static UserProfileDto ToUserProfileFromUser(this User user)
        {
            return new UserProfileDto
            {
                UserId = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Gender = user.Gender,
                Email = user.Email,
                Address = user.Address,
                ImageURL = user.ImageURL,
                ImagePublicId = user.ImagePublicId,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}
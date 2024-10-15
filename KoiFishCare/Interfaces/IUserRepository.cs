using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using KoiFishCare.Dtos.User;
using KoiFishCare.DTOs;
using KoiFishCare.DTOs.User;
using KoiFishCare.Models;

namespace KoiFishCare.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> UpdateAsync(string userID, UpdateUserProfileDTO userDTO);

        Task<List<User>?> GetAllUserAsync();

        Task<User?> GetUserByIdAsync(string userID);

        Task UpdateAsync(User user);
    }
}
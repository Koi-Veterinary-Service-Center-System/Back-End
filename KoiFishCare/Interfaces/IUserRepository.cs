using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.DTOs;
using KoiFishCare.DTOs.User;
using KoiFishCare.Models;

namespace KoiFishCare.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> UpdateAsync(string UserID, UpdateUserProfileDTO userDTO);
    }
}
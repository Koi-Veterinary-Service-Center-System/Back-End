using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Models;
using Microsoft.AspNetCore.Identity;

namespace KoiFishCare.Interfaces
{
    public interface ItokenService
    {
        string CreateToken(User user, IdentityRole role);
    }
}
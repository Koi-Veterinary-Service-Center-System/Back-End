using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace KoiFishCare.Interfaces
{
    public interface ItokenService
    {
        string CreateToken(User user);
    }
}
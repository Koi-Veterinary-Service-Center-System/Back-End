using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.User
{
    public class CreateUserDTO
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? Email { get; set; }

        public bool? Gender { get; set; }

        public string Role { get; set; } = null!;

    }
}
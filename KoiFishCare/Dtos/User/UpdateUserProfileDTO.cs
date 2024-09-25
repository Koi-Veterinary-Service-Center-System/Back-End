using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.User
{
    public class UpdateUserProfileDTO
    {
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool Gender { get; set; } = true;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int ExperienceYears { get; set; } = 0;
    }
}
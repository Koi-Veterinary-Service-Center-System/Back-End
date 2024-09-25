using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.User
{
    public class UpdateUserProfileDTO
    {
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? Gender { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? ImageURL { get; set; }
        public string? ImagePublicId { get; set; }
        public string? PhoneNumber { get; set; }
        public int? ExperienceYears { get; set; }
    }
}
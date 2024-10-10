using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.DTOs.User
{
    public class UpdateUserProfileDTO
    {
        [Required(ErrorMessage = "User Name is required!")]
        public string UserName { get; set; } = string.Empty;


        [Required(ErrorMessage = "First Name is required!")]
        public string FirstName { get; set; } = string.Empty;


        [Required(ErrorMessage = "Last Name is required!")]
        public string LastName { get; set; } = string.Empty;

        public bool? Gender { get; set; }
        
        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? ImageURL { get; set; }


        [Required(ErrorMessage = "Phone Number is required!")]
        public string PhoneNumber { get; set; } = string.Empty;

        public int? ExperienceYears { get; set; }
    }
}
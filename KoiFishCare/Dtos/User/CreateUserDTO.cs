using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.User
{
    public class CreateUserDTO
    {
        [Required(ErrorMessage = "First Name is required!")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last Name is required!")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "User Name is required!")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Password is required!")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public string? Email { get; set; }

        public bool? Gender { get; set; }

        [Required(ErrorMessage = "Role is required!")]
        public string Role { get; set; } = null!;

    }
}
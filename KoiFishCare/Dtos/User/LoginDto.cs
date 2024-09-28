using System.ComponentModel.DataAnnotations;

namespace KoiFishCare.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage ="Username is required")]
        public string? UserName {get; set;}

        [Required(ErrorMessage ="Password is required")]
        [DataType(DataType.Password)]
        public string? Password {get; set;}
    }
}
using System.ComponentModel.DataAnnotations;

namespace KoiFishCare.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage ="Username is required")]
        public string? UserName {get; set;}

        [Required(ErrorMessage ="Password is required")]
        [DataType(DataType.Password)]
        public string? Password {get; set;}
    }
}
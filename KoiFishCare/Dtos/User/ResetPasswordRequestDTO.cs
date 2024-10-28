using System.ComponentModel.DataAnnotations;

namespace KoiFishCare.DTOs
{
    public class ResetPasswordRequestDTO
    {
        public string Email { get; set; } = string.Empty;
    }
}
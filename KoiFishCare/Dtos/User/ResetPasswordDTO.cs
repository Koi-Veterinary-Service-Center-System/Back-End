using System.ComponentModel.DataAnnotations;

namespace KoiFishCare.DTOs
{
    public class ResetPasswordDTO
    {
        public string Email { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
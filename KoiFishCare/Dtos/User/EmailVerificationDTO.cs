using System.ComponentModel.DataAnnotations;

namespace KoiFishCare.DTOs
{
    public class EmailVerificationDTO
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }

}
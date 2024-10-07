using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.Feedback
{
    public class AddFeedbackDTO
    {
        [Required(ErrorMessage = "BookingId is required to create BookingID!")]
        public int BookingID { get; set; }

        public int? Rate { get; set; }

        public string? Comments { get; set; }

    }
}
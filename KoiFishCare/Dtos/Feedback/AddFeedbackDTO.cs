using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.Feedback
{
    public class AddFeedbackDTO
    {
        public int? Rate { get; set; }

        public string? Comments { get; set; }
    }
}
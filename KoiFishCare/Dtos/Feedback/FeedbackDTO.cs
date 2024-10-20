using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.Feedback
{
    public class FeedbackDTO
    {
        public int FeedbackID { get; set; }

        public int? BookingID { get; set; }

        public string? ServiceName { get; set; }

        public string? CustomerName { get; set; }

        public int? Rate { get; set; }

        public string? Comments { get; set; }

        public bool IsVisible { get; set; } = true;
    }
}
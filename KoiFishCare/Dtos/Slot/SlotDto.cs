using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.DTOs.Slot
{
    public class SlotDTO
    {
        public int? SlotID { get; set; }
        public TimeOnly? StartTime { get; set; }

        public TimeOnly? EndTime { get; set; }

        public string? WeekDate { get; set; }
    }
}
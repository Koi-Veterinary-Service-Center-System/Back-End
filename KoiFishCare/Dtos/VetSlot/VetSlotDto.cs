using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.VetSlot
{
    public class VetSlotDto
    {
        public bool? isBook { get; set; }
        public int? SlotID { get; set; }
        public TimeOnly? SlotStartTime { get; set; }
        public TimeOnly? SlotEndTime { get; set; }
        public string? WeekDate { get; set; }
        public string? VetId { get; set; }
        public string? VetName { get; set; }
        public string? VetFirstName { get; set; }
        public string? VetLastName { get; set; }
        public string? MeetURL { get; set; }
    }
}
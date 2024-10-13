using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using KoiFishCare.Models.Enum;

namespace KoiFishCare.Dtos.Slot
{
    public class CreateUpdateSlotDto
    {
        [Required]
        public string? StartTime { get; set; }
        [Required]
        public string? EndTime { get; set; }
        [Required]
        [Range(0,6)]
        public Models.Enum.DayOfWeek? WeekDate { get; set; }
    }
}
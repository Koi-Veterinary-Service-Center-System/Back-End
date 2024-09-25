using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Slot;
using KoiFishCare.Models;

namespace KoiFishCare.Mappers
{
    public static class SlotMappers
    {
        public static SlotDto ToSlotDto(this Slot slot)
        {
            return new SlotDto
            {
                SlotID = slot.SlotID,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime,
                WeekDate = slot.WeekDate
            };
        }
    }
}
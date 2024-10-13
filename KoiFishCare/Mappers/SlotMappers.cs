using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Slot;
using KoiFishCare.DTOs.Slot;
using KoiFishCare.Models;

namespace KoiFishCare.Mappers
{
    public static class SlotMappers
    {
        public static SlotDTO ToSlotDto(this Slot slot)
        {
            return new SlotDTO
            {
                SlotID = slot.SlotID,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime,
                WeekDate = slot.WeekDate.ToString()
            };
        }

        public static Slot ToSlotModel(this CreateUpdateSlotDto dto, TimeOnly startTime, TimeOnly endTime)
        {
            return new Slot
            {
                StartTime = startTime,
                EndTime = endTime,
                WeekDate = dto.WeekDate
            };
        }
    }
}
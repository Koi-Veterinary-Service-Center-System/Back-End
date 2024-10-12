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

        public static Slot ToSlotModel(this CreateUpdateSlotDto dto)
        {
            return new Slot
            {
                StartTime = TimeOnly.TryParse(dto.StartTime, out var parsedStartTime) ? parsedStartTime : throw new ArgumentException("Invalid StartTime"),
                EndTime = TimeOnly.TryParse(dto.EndTime, out var parsedEndTime) ? parsedEndTime : throw new ArgumentException("Invalid EndTime"),
                WeekDate = dto.WeekDate
            };
        }
    }
}
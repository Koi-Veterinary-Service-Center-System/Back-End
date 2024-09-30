using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.VetSlot;
using KoiFishCare.Models;

namespace KoiFishCare.Mappers
{
    public static class VetSlotMappers
    {
        public static VetSlotDto ToVetSlotDtoFromModel(this VetSlot vetSlot)
        {
            return new VetSlotDto
            {
                isBook = vetSlot.isBooked,
                SlotID = vetSlot.SlotID,
                SlotStartTime = vetSlot.Slot.StartTime,
                SlotEndTime = vetSlot.Slot.EndTime,
                WeekDate = vetSlot.Slot.WeekDate,
                VetId = vetSlot.Veterinarian.Id,
                VetName = vetSlot.Veterinarian.UserName,
                VetFirstName = vetSlot.Veterinarian.FirstName,
                VetLastName = vetSlot.Veterinarian.LastName
            };
        }
    }
}
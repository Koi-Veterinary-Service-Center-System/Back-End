using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.VetSlot;
using KoiFishCare.DTOs.Vet;
using KoiFishCare.Models;
using KoiFishCare.Models.Enum;

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
                SlotStartTime = vetSlot.Slot?.StartTime,
                SlotEndTime = vetSlot.Slot?.EndTime,
                WeekDate = vetSlot.Slot?.WeekDate.ToString(),
                VetId = vetSlot.Veterinarian?.Id,
                VetName = vetSlot.Veterinarian?.UserName,
                VetFirstName = vetSlot.Veterinarian?.FirstName,
                VetLastName = vetSlot.Veterinarian?.LastName,
                MeetURL = vetSlot.Veterinarian?.VetBookings.FirstOrDefault(b => b.SlotID == vetSlot.SlotID && b.BookingStatus != BookingStatus.Succeeded && b.BookingStatus != BookingStatus.Cancelled)?.MeetURL ?? null
            };
        }

        public static VetDTO ToVetDtoFromVetSlot(this VetSlot vs)
        {
            return new VetDTO
            {
                Id = vs.Veterinarian?.Id.ToString(),
                VetName = vs.Veterinarian?.UserName,
                FirstName = vs.Veterinarian?.FirstName,
                LastName = vs.Veterinarian?.LastName,
                Gender = vs.Veterinarian?.Gender,
                ExperienceYears = vs.Veterinarian?.ExperienceYears,
                ImageURL = vs.Veterinarian?.ImageURL,
                VetEmail = vs.Veterinarian?.Email
            };
        }
    }
}
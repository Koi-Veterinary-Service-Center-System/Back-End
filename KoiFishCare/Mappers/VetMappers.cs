using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.DTOs.Vet;
using KoiFishCare.Models;

namespace KoiFishCare.Mappers
{
    public static class VetMappers
    {
        public static VetDTO ToVetDto(this Veterinarian vet)
        {
            return new VetDTO
            {
                Id = vet.Id,
                VetName = vet.UserName,
                FirstName = vet.FirstName,
                LastName = vet.LastName,
                Gender = vet.Gender,
                ExperienceYears = vet.ExperienceYears,
                ImageURL = vet.ImageURL,
                VetEmail = vet.Email
            };
        }
    }
}
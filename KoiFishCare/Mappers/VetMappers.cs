using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Vet;
using KoiFishCare.Models;

namespace KoiFishCare.Mappers
{
    public static class VetMappers
    {
        public static VetDto ToVetDto(this Veterinarian vet)
        {
            return new VetDto
            {
                Id = vet.Id,
                VetName = vet.UserName,
                FirstName = vet.FirstName,
                LastName = vet.LastName,
                Gender = vet.Gender,
                ExperienceYears = vet.ExperienceYears
            };
        }
    }
}
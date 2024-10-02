using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.DTOs.Vet
{
    public class VetDTO
    {
        public string? Id { get; set; }
        public string? VetName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? Gender { get; set; }
        public int? ExperienceYears { get; set; }
        public string? ImageURL { get; set; }
    }
}
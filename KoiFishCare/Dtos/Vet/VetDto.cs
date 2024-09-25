using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.Vet
{
    public class VetDto
    {
        public string Id { get; set; } = string.Empty;
        public string VetName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool? Gender { get; set; }
        public int? ExperienceYears { get; set; }
    }
}
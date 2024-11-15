using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.PrescriptionRecord
{
    public class UpdatePresRecordDto
    {
        public string? DiseaseName { get; set; }

        public string? Symptoms { get; set; }

        public string? MedicationDetails { get; set; }

        public string? Note { get; set; }
    }
}
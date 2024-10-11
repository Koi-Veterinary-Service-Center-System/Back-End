using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.PrescriptionRecord
{
    public class PrescriptionRecordDto
    {
        public int PrescriptionRecordID { get; set; }
        
        public string? DiseaseName { get; set; }

        public string? Symptoms { get; set; }

        public string? Medication { get; set; }

        public string? Frequency { get; set; }

        public string? Note { get; set; }

        public decimal? RefundMoney { get; set; }

        public decimal? RefundPercent { get; set; }

        public int? BookingID { get; set; }

        public DateTime CreateAt { get; set; }
    }
}
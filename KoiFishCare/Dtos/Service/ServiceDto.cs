using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.Service
{
    public class ServiceDto
    {
        public int? ServiceID { get; set; }
        public string? ServiceName { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public double? EstimatedDuration { get; set; }
    }
}
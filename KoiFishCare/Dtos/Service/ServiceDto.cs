using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.DTOs.Service
{
    public class ServiceDTO
    {
        public int? ServiceID { get; set; }
        public string? ServiceName { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? QuantityPrice { get; set; }
        public double? EstimatedDuration { get; set; }
        public string? ImageURL { get; set; }
        public bool IsAtHome { get; set; }
        public bool IsDeleted { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.KoiOrPool
{
    public class KoiOrPoolDto
    {
        public int? KoiOrPoolID { get; set; }
        public string? Name { get; set; }
        public bool? IsPool { get; set; }
        public string? Description { get; set; }
        public string? CustomerId { get; set; }
    }
}
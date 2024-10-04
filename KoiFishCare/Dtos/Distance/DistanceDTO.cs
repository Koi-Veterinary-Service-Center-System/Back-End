using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.Distance
{
    public class DistanceDTO
    {
        public int DistanceID { get; set; }
        public string? District { get; set; }
        public string? Area { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.Distance
{
    public class AddDistanceDTO
    {
        [Required(ErrorMessage = "District is required!")]
        public string? District { get; set; }

        [Required(ErrorMessage = "Area is required!")]
        public string? Area { get; set; }

        [Required(ErrorMessage = "Price is required!")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
    }
}
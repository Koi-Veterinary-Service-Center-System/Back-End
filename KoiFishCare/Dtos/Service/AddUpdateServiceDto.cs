using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.Service
{
    public class AddUpdateServiceDto
    {
        [Required(ErrorMessage = "Service name is required.")]
        public string ServiceName { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Duration is required.")]
        public double EstimatedDuration { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.DTOs.Service
{
    public class AddUpdateServiceDTO
    {
        [Required(ErrorMessage = "Service name is required.")]
        public string ServiceName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "Price is required.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Quantity Price is required.")]
        public decimal QuantityPrice { get; set; }

        [Required(ErrorMessage = "Duration is required.")]
        public double EstimatedDuration { get; set; }
        public string? ImageURL { get; set; }

        [Required(ErrorMessage = "Is At Home is required.")]
        public bool IsAtHome { get; set; }
    }
}
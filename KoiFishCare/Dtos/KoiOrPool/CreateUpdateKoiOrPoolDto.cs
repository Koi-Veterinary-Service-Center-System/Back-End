using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.KoiOrPool
{
    public class CreateUpdateKoiOrPoolDto
    {
        public string? Name { get; set; }
        [Required(ErrorMessage = "Please spectify this is koi or pool!")]
        public bool? IsPool { get; set; }
        public string? Description { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.VetSlot
{
    public class UpdateVetSlotDto
    {
        [Required(ErrorMessage = "VetId is required")]
        public string VetID { get; set; } = null!;
        [Required(ErrorMessage = "SlotId is required")]
        public int SlotID { get; set; }
        [Required(ErrorMessage = "Enter vetslot is booked or not!")]
        public bool isBook{ get; set; }
    }
}
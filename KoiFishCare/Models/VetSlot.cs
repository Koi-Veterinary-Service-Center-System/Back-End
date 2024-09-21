using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace KoiFishCare.Models
{
    public class VetSlot
    {
        public string VetId { get; set; }
        public int SlotId { get; set; }
        public Veterinarian Veterinarian { get; set; }
        public Slot Slot { get; set; }
    }
}
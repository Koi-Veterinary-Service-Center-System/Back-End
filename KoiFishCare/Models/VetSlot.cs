using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Models;
// Warning
[Table("VetSlots")]
public class VetSlot
{
    [ForeignKey("VetID")]
    public required string VetID { get; set; }
    [ForeignKey("SlotID")]
    public required int SlotID { get; set; }
    public Veterinarian? Veterinarian { get; set; }
    public Slot? Slot { get; set; }
    public bool isBooked { get; set; }
}
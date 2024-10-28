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
    public bool? isBooked { get; set; }

    // ---- Veterinarian -----------------------------------------------------------------------
    [ForeignKey("VetID")]
    public string? VetID { get; set; }
    public User? Veterinarian { get; set; }

    // ---- Slot -----------------------------------------------------------------------
    [ForeignKey("SlotID")]
    public int SlotID { get; set; }
    public Slot? Slot { get; set; }

}
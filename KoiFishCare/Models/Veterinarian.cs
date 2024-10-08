using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoiFishCare.Models;

[Table("Veterinarians")]
public partial class Veterinarian : User
{
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<VetSlot> VetSlots { get; set; } = new List<VetSlot>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using KoiFishCare.Models;

namespace WebApplication1.Models;

[Table("Veterinarians")]
public partial class Veterinarian : User
{
    public int ExperienceYears { get; set; }

    // public virtual Account Account { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<VetSlot> VetSlots { get; set; } = new List<VetSlot>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KoiFishCare.Models;
using KoiFishCare.Models.Enum;

namespace KoiFishCare.Models;

[Table("Slots")]
public partial class Slot
{
    [Key]
    public int SlotID { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public Enum.DayOfWeek? WeekDate { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    
    public virtual ICollection<VetSlot> VetSlots { get; set; } = new List<VetSlot>();
}

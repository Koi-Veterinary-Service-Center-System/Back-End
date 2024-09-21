using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Slot
{
    public int SlotId { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public DateOnly? WeekDate { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Veterinarian> Vets { get; set; } = new List<Veterinarian>();
}

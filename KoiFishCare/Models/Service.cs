using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public string? ServiceName { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? EstimatedDuration { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Distance
{
    public int DistanceId { get; set; }

    public decimal? Price { get; set; }

    public decimal? Kilometer { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

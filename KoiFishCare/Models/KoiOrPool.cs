using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class KoiOrPool
{
    public string KoiOrPoolId { get; set; } = null!;

    public string? CustomerId { get; set; }
    
    public int? BookingId { get; set; }

    public string? Name { get; set; }

    public bool? IsPool { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Customer? Customer { get; set; }
}

using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public string? Qrcode { get; set; }

    public string? Type { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

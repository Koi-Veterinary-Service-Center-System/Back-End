using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? BookingId { get; set; }

    public int? Rate { get; set; }

    public string? Comments { get; set; }

    public virtual Booking? Booking { get; set; }
}

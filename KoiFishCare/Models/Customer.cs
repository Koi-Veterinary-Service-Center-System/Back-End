using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Customer
{
    public string CustomerId { get; set; } = null!;

    public int? AccountId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Gender { get; set; }

    public string? PhoneNumber { get; set; }

    public string? DefaultAddress { get; set; }

    public string? ImageUrl { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<KoiOrPool> KoiOrPools { get; set; } = new List<KoiOrPool>();
}

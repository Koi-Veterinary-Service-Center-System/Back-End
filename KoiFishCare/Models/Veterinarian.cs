using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Veterinarian
{
    public string VetId { get; set; } = null!;

    public int? AccountId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Gender { get; set; }

    public string? PhoneNumber { get; set; }

    public int? ExperienceYears { get; set; }

    public string? ImageUrl { get; set; }

    public virtual Account? Account { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();
}

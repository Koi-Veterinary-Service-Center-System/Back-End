using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;

[Table("Customers")]
public partial class Customer : User
{
    public string DefaultAddress { get; set; } = string.Empty;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

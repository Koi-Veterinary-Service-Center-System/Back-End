using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoiFishCare.Models;

[Table("Customers")]
public partial class Customer : User
{
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

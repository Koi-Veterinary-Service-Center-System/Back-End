using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;

[Table("Services")]
public partial class Service
{
    [Key]
    public int ServiceID { get; set; }

    public string ServiceName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;


    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    public double EstimatedDuration { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

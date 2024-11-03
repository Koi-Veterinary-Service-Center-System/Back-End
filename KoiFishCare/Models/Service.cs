using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoiFishCare.Models;

[Table("Services")]
public partial class Service
{
    [Key]
    public int ServiceID { get; set; }

    public string ServiceName { get; set; } = null!;

    public string? ImageURL { get; set; }

    public string Description { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal QuantityPrice { get; set; }

    public double EstimatedDuration { get; set; }

    public bool IsAtHome { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

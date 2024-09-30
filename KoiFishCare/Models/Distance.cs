using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoiFishCare.Models;
[Table("Distances")]
public partial class Distance
{
    [Key]
    public int DistanceID { get; set; }
    public string? District { get; set; }
    public string? Area { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

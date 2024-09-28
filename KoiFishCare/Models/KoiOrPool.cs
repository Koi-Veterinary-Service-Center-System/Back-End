using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoiFishCare.Models;
[Table("KoiOrPools")]
public partial class KoiOrPool
{
    [Key]
    public int KoiOrPoolID { get; set; }

    public string? Name { get; set; }

    public bool? IsPool { get; set; }

    public string? Description { get; set; }

    //Customer
    [ForeignKey("CustomerID")]
    public string CustomerID { get; set; } = null!;
    
    public Customer Customer { get; set; } = null!;

    // ---- Booking -----------------------------------------------------------------------
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

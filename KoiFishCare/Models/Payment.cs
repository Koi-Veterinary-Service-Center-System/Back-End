using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoiFishCare.Models;

[Table("Payments")]
public partial class Payment
{
    [Key]
    public int PaymentID { get; set; }

    public string? Qrcode { get; set; }

    public string Type { get; set; } = null!;

    // ---- Booking -----------------------------------------------------------------------
    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;
[Table("KoiOrPools")]
public partial class KoiOrPool
{
    [Key]
    public required int KoiOrPoolID { get; set; }

    public string? Name { get; set; }

    public bool IsPool { get; set; }

    public string? Description { get; set; }

    // ---- Booking -----------------------------------------------------------------------
    // Cần xem lại, chưa hiểu vì sao 1 koi/pool có nhiều booking

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();






}

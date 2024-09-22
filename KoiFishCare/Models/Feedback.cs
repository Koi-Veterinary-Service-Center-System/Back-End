using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;
[Table("Feedbacks")]
public partial class Feedback
{
    [Key]
    public int FeedbackID { get; set; }

    public int? Rate { get; set; }

    public string? Comments { get; set; }

    // ---- Booking -----------------------------------------------------------------------
    [ForeignKey("BookingID")]
    public int? BookingID { get; set; }

    public virtual Booking? Booking { get; set; }

}

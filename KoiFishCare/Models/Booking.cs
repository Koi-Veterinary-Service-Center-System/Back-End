using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KoiFishCare.Models.Enum;

namespace WebApplication1.Models;

[Table("Bookings")]
public partial class Booking
{
    [Key]
    public int BookingID { get; set; }

    public DateOnly? BookingDate { get; set; }

    public TimeOnly? BookingTime { get; set; }

    public string? Location { get; set; }

    public string? Note { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TotalAmount { get; set; }

    public BookingStatus BookingStatus { get; set; }

    // ---- Payment -----------------------------------------------------------------------
    [ForeignKey("PaymentID")]
    public required int PaymentID { get; set; }
    public virtual required Payment Payment { get; set; }

    // ---- Service -----------------------------------------------------------------------
    [ForeignKey("ServiceID")]
    public required int ServiceID { get; set; }

    public virtual required Service Service { get; set; }

    // ---- Slot -----------------------------------------------------------------------
    [ForeignKey("SlotID")]
    public required int SlotID { get; set; }

    public virtual required Slot Slot { get; set; }

    // ---- Customer -----------------------------------------------------------------------
    [ForeignKey("CustomerID")]
    public required string CustomerID { get; set; }

    public virtual required Customer Customer { get; set; }

    // ---- Vet -----------------------------------------------------------------------
    [ForeignKey("VetID")]
    public required string VetID { get; set; }

    public virtual required Veterinarian Vet { get; set; }

    // ---- Distance -----------------------------------------------------------------------
    [ForeignKey("DistanceID")]
    public required int DistanceID { get; set; }

    public virtual required Distance Distance { get; set; }

    // ---- KoiOrPool -----------------------------------------------------------------------
    [ForeignKey("KoiOrPoolID")]
    public required int KoiOrPoolID { get; set; }

    public virtual required KoiOrPool KoiOrPool { get; set; }
}

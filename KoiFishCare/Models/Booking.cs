using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KoiFishCare.Models.Enum;

namespace KoiFishCare.Models;

[Table("Bookings")]
public partial class Booking
{
    [Key]
    public int BookingID { get; set; }

    public DateOnly? BookingDate { get; set; }

    public string? Location { get; set; }

    public string? Note { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? TotalAmount { get; set; }

    public BookingStatus BookingStatus { get; set; }

    public string? MeetURL { get; set; }

    // ---- Payment -----------------------------------------------------------------------
    [ForeignKey("PaymentID")]
    public  int? PaymentID { get; set; }
    public Payment? Payment { get; set; }

    // ---- Service -----------------------------------------------------------------------
    [ForeignKey("ServiceID")]
    public int? ServiceID { get; set; }

    public Service? Service { get; set; }

    // ---- Slot -----------------------------------------------------------------------
    [ForeignKey("SlotID")]
    public int? SlotID { get; set; }

    public Slot? Slot { get; set; }

    // ---- Customer -----------------------------------------------------------------------
    [ForeignKey("CustomerID")]
    public string CustomerID { get; set; } = string.Empty;

    public Customer? Customer { get; set; }

    // ---- Vet -----------------------------------------------------------------------
    [ForeignKey("VetID")]
    public string VetID { get; set; } = string.Empty;

    public Veterinarian? Veterinarian { get; set; }

    // ---- Distance -----------------------------------------------------------------------
    [ForeignKey("DistanceID")]
    public int? DistanceID { get; set; }

    public Distance? Distance { get; set; }

    // ---- KoiOrPool -----------------------------------------------------------------------
    [ForeignKey("KoiOrPoolID")]
    public int? KoiOrPoolID { get; set; }

    public KoiOrPool? KoiOrPool { get; set; }
}

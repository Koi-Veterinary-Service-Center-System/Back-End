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

    public DateOnly BookingDate { get; set; }

    public string? Location { get; set; }

    public string? Note { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal InitAmount { get; set; }

    public int Quantity { get; set; }

    public BookingStatus BookingStatus { get; set; }

    public bool isPaid { get; set; } = false;

    public bool hasPres { get; set; } = false;

    public string? MeetURL { get; set; }

    // ---- Payment -----------------------------------------------------------------------
    [ForeignKey("PaymentID")]
    public int PaymentID { get; set; }
    public Payment Payment { get; set; } = null!;

    public string? PaymentTypeAtBooking { get; set; }

    // ---- Service -----------------------------------------------------------------------
    [ForeignKey("ServiceID")]
    public int ServiceID { get; set; }

    public Service Service { get; set; } = null!;

    public string? ServiceNameAtBooking { get; set; }
    public decimal ServicePriceAtBooking { get; set; }
    public decimal ServiceQuantityPriceAtBooking { get; set; }

    // ---- Slot -----------------------------------------------------------------------
    [ForeignKey("SlotID")]
    public int SlotID { get; set; }

    public Slot Slot { get; set; } = null!;

    public TimeOnly? SlotStartTimeAtBooking { get; set; }
    public TimeOnly? SlotEndTimeAtBooking { get; set; }
    public string? SlotWeekDateAtBooking { get; set; } = null!;

    // ---- Customer -----------------------------------------------------------------------
    [ForeignKey("CustomerID")]
    public string CustomerID { get; set; } = null!;

    public User Customer { get; set; } = null!;

    // ---- Vet -----------------------------------------------------------------------
    [ForeignKey("VetID")]
    public string VetID { get; set; } = null!;

    public User Veterinarian { get; set; } = null!;

    // ---- Prescription Record -----------------------------------------------------------------------
    public virtual ICollection<PrescriptionRecord> PrescriptionRecords { get; set; } = new List<PrescriptionRecord>();

    // ---- Booking Record -----------------------------------------------------------------------
    public virtual BookingRecord? BookingRecord { get; set; }

    // ---- Feedback -----------------------------------------------------------------------
    public virtual Feedback? Feedback { get; set; }

}

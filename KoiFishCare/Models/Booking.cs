using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? ServiceId { get; set; }

    public int? PaymentId { get; set; }

    public int? SlotId { get; set; }

    public string? CustomerId { get; set; }

    public string? VetId { get; set; }

    public int? DistanceId { get; set; }

    public string? KoiOrPoolId { get; set; }

    public DateOnly? BookingDate { get; set; }

    public TimeOnly? BookingTime { get; set; }

    public string? Location { get; set; }

    public string? Note { get; set; }

    public decimal? TotalAmount { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Distance? Distance { get; set; }

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual KoiOrPool? KoiOrPool { get; set; }

    public virtual Payment? Payment { get; set; }

    public virtual ICollection<PrescriptionRecord> PrescriptionRecords { get; set; } = new List<PrescriptionRecord>();

    public virtual Service? Service { get; set; }

    public virtual Slot? Slot { get; set; }

    public virtual Veterinarian? Vet { get; set; }
}

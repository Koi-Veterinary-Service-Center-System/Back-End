using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class PrescriptionRecord
{
    public int PrescriptionRecordId { get; set; }

    public int? BookingId { get; set; }

    public string? DiseaseName { get; set; }

    public string? Symptoms { get; set; }

    public string? Medication { get; set; }

    public string? Note { get; set; }

    public virtual Booking? Booking { get; set; }
}

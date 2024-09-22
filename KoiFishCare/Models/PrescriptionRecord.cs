using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;

[Table("PrescriptionRecords")]
public partial class PrescriptionRecord
{
    [Key]
    public int? PrescriptionRecordID { get; set; }

    public string? DiseaseName { get; set; }

    public string? Symptoms { get; set; }

    public string? Medication { get; set; }

    public string? Note { get; set; }

    // ---- Booking -----------------------------------------------------------------------
    public int? BookingID { get; set; }

    public virtual Booking? Booking { get; set; }
}

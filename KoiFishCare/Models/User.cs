using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace KoiFishCare.Models;

[Table("Users")]
public partial class User : IdentityUser
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public bool? Gender { get; set; }

    public string? Address { get; set; }

    public string? ImageURL { get; set; }

    public string? ImagePublicId { get; set; } //id của hình lưu trên cloud

    public bool isBanned { get; set; }

    // ---- Attribute for Vet -----------------------------------------------------------------------
    public int? ExperienceYears { get; set; }

    public virtual ICollection<VetSlot>? VetSlots { get; set; } = new List<VetSlot>();

    public virtual ICollection<Booking> VetBookings { get; set; } = new List<Booking>();

    // ---- Attributes for Staff and Manager -----------------------------------------------------------------------
    public string? ManagerID { get; set; }

    public bool IsManager { get; set; }

    // ---- Attribute for Customer -----------------------------------------------------------------------
    public virtual ICollection<Booking> CustomerBookings { get; set; } = new List<Booking>();
}

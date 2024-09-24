using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace KoiFishCare.Models;

[Table("Users")]
public partial class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public bool? Gender { get; set; }

    public string? Address { get; set; }

    public int? ExperienceYears { get; set; }

    public string? ImageURL { get; set; }

    public string? ImagePublicId {get;set;} //id của hình lưu trên cloud

    // public virtual Staff Staff { get; set; } = null!;

    // public virtual Customer Customer { get; set; } = null!;

    // public virtual Veterinarian Veterinarian { get; set; } = null!;
}

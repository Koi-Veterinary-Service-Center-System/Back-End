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

    public int? ExperienceYears { get; set; }

    public string? ImageURL { get; set; }

    public string? ImagePublicId { get; set; } //id của hình lưu trên cloud

}

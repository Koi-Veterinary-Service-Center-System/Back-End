using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models;

[Table("Users")]
public partial class User : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public bool Gender { get; set; }

    public string? ImageURL { get; set; } = string.Empty;

    public string ImagePublicId {get;set;} = string.Empty; //id của hình lưu trên cloud

    // public virtual Staff Staff { get; set; } = null!;

    // public virtual Customer Customer { get; set; } = null!;

    // public virtual Veterinarian Veterinarian { get; set; } = null!;
}

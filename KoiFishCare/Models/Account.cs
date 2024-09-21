using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int? Role { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Staff? Staff { get; set; }

    public virtual Veterinarian? Veterinarian { get; set; }
}

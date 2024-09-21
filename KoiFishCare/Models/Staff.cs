using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Staff
{
    public string StaffId { get; set; } = null!;

    public int? AccountId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Gender { get; set; }

    public string? PhoneNumber { get; set; }

    public string? ImageUrl { get; set; }

    public bool? IsManager { get; set; }

    public virtual Account? Account { get; set; }
}

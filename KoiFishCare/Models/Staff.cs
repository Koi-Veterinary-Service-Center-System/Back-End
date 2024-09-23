using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoiFishCare.Models;

[Table("Staffs")]
public partial class Staff : User
{
    public string? ManagerID { get; set; }  
    public bool? IsManager { get; set; }

    // public string AccountId { get; set; } = null!;

    // public virtual Account Account { get; set; } = null!;
}

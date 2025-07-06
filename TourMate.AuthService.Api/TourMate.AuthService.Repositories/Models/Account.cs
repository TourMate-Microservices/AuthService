using System;
using System.Collections.Generic;

namespace TourMate.AuthService.Repositories.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public int RoleId { get; set; }

    public bool Status { get; set; }

    public virtual Role Role { get; set; } = null!;
}

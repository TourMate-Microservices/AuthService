﻿using System;
using System.Collections.Generic;

namespace TourMate.AuthService.Repositories.Models;

public partial class RefreshToken
{
    public Guid Id { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime ExpireAt { get; set; }

    public bool IsRevoked { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Account User { get; set; } = null!;
}

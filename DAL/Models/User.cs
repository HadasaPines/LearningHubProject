using System;
using System.Collections.Generic;

namespace DAL.Models;

public class User
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public virtual Student? Student { get; set; }

    public virtual Teacher? Teacher { get; set; }
}

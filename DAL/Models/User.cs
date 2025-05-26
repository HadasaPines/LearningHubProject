using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Index("Email", Name = "UQ__Users__A9D105347122B63E", IsUnique = true)]
public partial class User
{
    [Key]
    public int UserId { get; set; }

    [StringLength(100)]
    public string FullName { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    [InverseProperty("StudentNavigation")]
    public virtual Student? Student { get; set; }

    [InverseProperty("TeacherNavigation")]
    public virtual Teacher? Teacher { get; set; }
}

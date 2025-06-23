using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

public partial class Student
{
    [Key]
    public int StudentId { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Gender { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public int? Age { get; set; }

    [InverseProperty("Student")]
    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    [ForeignKey("StudentId")]
    [InverseProperty("Student")]
    public virtual User StudentNavigation { get; set; } = null!;
}

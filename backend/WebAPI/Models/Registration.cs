using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

public partial class Registration
{
    [Key]
    public int RegistrationId { get; set; }

    public int LessonId { get; set; }

    public int StudentId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime RegistrationDate { get; set; }

    [ForeignKey("LessonId")]
    [InverseProperty("Registrations")]
    public virtual Lesson Lesson { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("Registrations")]
    public virtual Student Student { get; set; } = null!;
}

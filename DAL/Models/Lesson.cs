using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

[Index("Status", "LessonDate", Name = "IX_Lessons_Status_Date")]
public partial class Lesson
{
    [Key]
    public int LessonId { get; set; }

    public int TeacherId { get; set; }

    public int SubjectId { get; set; }

    public DateOnly LessonDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int? MinAge { get; set; }

    public int? MaxAge { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Gender { get; set; } = null!;

    [StringLength(20)]
    public string Status { get; set; } = null!;

    [InverseProperty("Lesson")]
    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    [ForeignKey("SubjectId")]
    [InverseProperty("Lessons")]
    public virtual Subject Subject { get; set; } = null!;

    [ForeignKey("TeacherId")]
    [InverseProperty("Lessons")]
    public virtual Teacher Teacher { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

public partial class Teacher
{
    [Key]
    public int TeacherId { get; set; }

    public string? Bio { get; set; }

    [StringLength(20)]
    public string Phone { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string Gender { get; set; } = null!;

    public DateOnly? BirthDate { get; set; }

    [InverseProperty("Teacher")]
    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    [InverseProperty("Teacher")]
    public virtual ICollection<TeacherAvailability> TeacherAvailabilities { get; set; } = new List<TeacherAvailability>();

    [ForeignKey("TeacherId")]
    [InverseProperty("Teacher")]
    public virtual User TeacherNavigation { get; set; } = null!;

    [InverseProperty("Teacher")]
    public virtual ICollection<TeachersToSubject> TeachersToSubjects { get; set; } = new List<TeachersToSubject>();
}

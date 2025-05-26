using System;
using System.Collections.Generic;


namespace DAL.Models;

public partial class Teacher
{
    public int TeacherId { get; set; }

    public string? Bio { get; set; }

    public string Phone { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public DateOnly? BirthDate { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public virtual ICollection<TeacherAvailability> TeacherAvailabilities { get; set; } = new List<TeacherAvailability>();

    public virtual User TeacherNavigation { get; set; } = null!;


    public virtual ICollection<TeachersToSubject> TeachersToSubjects { get; set; } = new List<TeachersToSubject>();
}

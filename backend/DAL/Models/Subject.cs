using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Subject
{
    public int SubjectId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

    public virtual ICollection<TeachersToSubject> TeachersToSubjects { get; set; } = new List<TeachersToSubject>();
}

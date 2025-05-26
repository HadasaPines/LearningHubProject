using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Registration
{
    public int RegistrationId { get; set; }

    public int LessonId { get; set; }

    public int StudentId { get; set; }

    public DateTime RegistrationDate { get; set; }

    public virtual Lesson Lesson { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}

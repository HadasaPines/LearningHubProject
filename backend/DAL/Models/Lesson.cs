using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Lesson
{
   
    public int LessonId { get; set; }

    public int TeacherId { get; set; }

    public int SubjectId { get; set; }

    public DateOnly LessonDate { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public int? MinAge { get; set; }

    public int? MaxAge { get; set; }

    public string Gender { get; set; } = null!;

   
    public string Status { get; set; } = null!;

    
    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

   
    public virtual Subject Subject { get; set; } = null!;


    public virtual Teacher Teacher { get; set; } = null!;

}

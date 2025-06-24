using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class TeacherAvailability
{
    public int AvailabilityId { get; set; }

    public int TeacherId { get; set; }

    public int WeekDay { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public virtual Teacher Teacher { get; set; } = null!;
}

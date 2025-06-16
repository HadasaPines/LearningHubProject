using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

[Table("TeacherAvailability")]
[Index("TeacherId", "WeekDay", Name = "IX_TeacherAvailability_TeacherId_WeekDay")]
public partial class TeacherAvailability
{
    [Key]
    public int AvailabilityId { get; set; }

    public int TeacherId { get; set; }

    public int WeekDay { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    [ForeignKey("TeacherId")]
    [InverseProperty("TeacherAvailabilities")]
    public virtual TeacherAPI Teacher { get; set; } = null!;
}

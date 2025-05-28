using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Models;

public partial class TeachersToSubject
{
    [Key]
    public int Id { get; set; }

    public int TeacherId { get; set; }

    public int SubjectId { get; set; }

    [ForeignKey("SubjectId")]
    [InverseProperty("TeachersToSubjects")]
    public virtual Subject Subject { get; set; } = null!;

    [ForeignKey("TeacherId")]
    [InverseProperty("TeachersToSubjects")]
    public virtual Teacher Teacher { get; set; } = null!;
}

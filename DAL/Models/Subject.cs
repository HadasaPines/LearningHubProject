using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public partial class Subject
{
    [Key]
    public int SubjectId { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    [InverseProperty("Subject")]
    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}

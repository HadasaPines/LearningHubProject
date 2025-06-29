using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DAL.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string Gender { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

   
    public int? Age { get; set; }

    public virtual ICollection<Registration> Registrations { get; set; } = new List<Registration>();

    [JsonIgnore]
    public virtual User StudentNavigation { get; set; } = null!;
}

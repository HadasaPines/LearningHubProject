using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public partial class StudentSubscription
{
    [Key]
    public int StudentSubscriptionId { get; set; }

    public int StudentId { get; set; }

    public int SubscriptionId { get; set; }

    public int LessonsUsed { get; set; }

    public bool IsActive { get; set; }

    [ForeignKey("StudentId")]
    [InverseProperty("StudentSubscriptions")]
    public virtual Student Student { get; set; } = null!;

    [ForeignKey("SubscriptionId")]
    [InverseProperty("StudentSubscriptions")]
    public virtual Subscription Subscription { get; set; } = null!;
}

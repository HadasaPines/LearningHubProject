using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public partial class LearningHubDbContext : DbContext
{
    public LearningHubDbContext()
    {
    }

    public LearningHubDbContext(DbContextOptions<LearningHubDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<TeacherAvailability> TeacherAvailabilities { get; set; }

    public virtual DbSet<TeachersToSubject> TeachersToSubjects { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\User\\Documents\\dvora\\פרויקט סופי\\LearningHubProject\\backend\\DAL\\database\\LearningHubDB.mdf;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.LessonId).HasName("PK__Lessons__B084ACD0476C1339");

            entity.Property(e => e.Gender)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Status)
                .HasDefaultValue("Available")
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.Subject).WithMany(p => p.Lessons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lessons_Subjects");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Lessons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lessons_Teachers");
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.RegistrationId).HasName("PK__Registra__6EF588101EC4BB5C");

            entity.Property(e => e.RegistrationDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Lesson).WithOne(p => p.Registration)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Registrations_Lessons");

            entity.HasOne(d => d.Student).WithMany(p => p.Registrations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Registrations_Students");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52B992EDBEF62");

            entity.Property(e => e.StudentId).ValueGeneratedNever();
            entity.Property(e => e.Gender)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.StudentNavigation).WithOne(p => p.Student)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Students_Users");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PK__Subjects__AC1BA3A8209A45EB");

            entity.Property(e => e.Name).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK__Teachers__EDF2596463D03245");

            entity.Property(e => e.TeacherId).ValueGeneratedNever();
            entity.Property(e => e.Bio).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Gender)
                .IsFixedLength()
                .UseCollation("SQL_Latin1_General_CP1_CI_AS");

            entity.HasOne(d => d.TeacherNavigation).WithOne(p => p.Teacher)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Teachers_Users");
        });

        modelBuilder.Entity<TeacherAvailability>(entity =>
        {
            entity.HasKey(e => e.AvailabilityId).HasName("PK__TeacherA__DA3979B104A3882A");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TeacherAvailabilities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeacherAvailability_Teachers");
        });

        modelBuilder.Entity<TeachersToSubject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Teachers__3214EC073C53DA1C");

            entity.HasOne(d => d.Subject).WithMany(p => p.TeachersToSubjects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeachersToSubjects_ToTable_1");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TeachersToSubjects)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeachersToSubjects_ToTable");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__tmp_ms_x__1788CC4C790A0DFA");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.Email).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.FirstName).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.LastName).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.PasswordHash).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Phone).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            entity.Property(e => e.Role).UseCollation("SQL_Latin1_General_CP1_CI_AS");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

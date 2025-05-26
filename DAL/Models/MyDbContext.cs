using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    public virtual DbSet<TeacherAvailability> TeacherAvailabilities { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\User\\Documents\\dvora\\LearningHubProject\\LearningHub\\DAL\\database\\projectDB.mdf;Integrated Security=True;Connect Timeout=30;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.LessonId).HasName("PK__Lessons__B084ACD09339D17C");

            entity.Property(e => e.Gender).IsFixedLength();
            entity.Property(e => e.Status).HasDefaultValue("Available");

            entity.HasOne(d => d.Subject).WithMany(p => p.Lessons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lessons_Subjects");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Lessons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lessons_Teachers");
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.RegistrationId).HasName("PK__Registra__6EF588107DCDCD25");

            entity.Property(e => e.RegistrationDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Lesson).WithMany(p => p.Registrations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Registrations_Lessons");

            entity.HasOne(d => d.Student).WithMany(p => p.Registrations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Registrations_Students");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__Students__32C52B9961F9D6E0");

            entity.Property(e => e.StudentId).ValueGeneratedNever();
            entity.Property(e => e.Gender).IsFixedLength();

            entity.HasOne(d => d.StudentNavigation).WithOne(p => p.Student)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Students_Users");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PK__Subjects__AC1BA3A8E48D8BD9");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK__Teachers__EDF259648C2B3F02");

            entity.Property(e => e.TeacherId).ValueGeneratedNever();
            entity.Property(e => e.Gender).IsFixedLength();

            entity.HasOne(d => d.TeacherNavigation).WithOne(p => p.Teacher)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Teachers_Users");
        });

        modelBuilder.Entity<TeacherAvailability>(entity =>
        {
            entity.HasKey(e => e.AvailabilityId).HasName("PK__TeacherA__DA3979B10A85D472");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TeacherAvailabilities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TeacherAvailability_Teachers");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CDC16AC1C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

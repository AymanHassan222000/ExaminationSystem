using ExaminationSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ExaminationSystem.Data;

public class Context : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSqlServer("Server=.;Database=ExaminationSystemDB;Trusted_Connection=True;TrustServerCertificate=True;")
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .LogTo(log => Debug.WriteLine(log),LogLevel.Information)
            .EnableSensitiveDataLogging(true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //modelBuilder.Entity<StudentCourse>()
        //    .HasIndex(e => new { e.StudetnID, e.CourseID })
        //    .IsUnique();

        modelBuilder.Entity<ExamQuestion>()
            .HasIndex(e => new { e.ExamID, e.QuestionID })
            .IsUnique();

        modelBuilder.Entity<ExamAttempt>()
            .HasIndex(e => new { e.StudentID, e.ExamID })
            .IsUnique();

        modelBuilder.Entity<Choice>()
            .HasIndex(c => c.QuestionID)
            .IsUnique()
            .HasFilter("[IsCorrect] = 1");
    }

    public DbSet<Course> Courses { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<StudentCourse> StudentCourses { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<ExamQuestion> ExamQuestions { get; set; }
    public DbSet<Choice> Choices { get; set; }
    public DbSet<ExamAttempt> ExamAttempts { get; set; }
    public DbSet<ExamAttemptAnswer> ExamAttemptAnswers { get; set; }

    public DbSet<ExamResult> ExamResults { get; set; }
}

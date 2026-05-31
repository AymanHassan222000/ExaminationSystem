using ExaminationSystem.DAL.Models;
using ExaminationSystem.DAL.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.DAL.Context;

public class ExaminationSystemDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService;
    public ExaminationSystemDbContext(
        DbContextOptions<ExaminationSystemDbContext> options,
        ICurrentUserService currentUserService
    ) : base(options)
    {
        _currentUserService = currentUserService;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExaminationSystemDbContext).Assembly);

        base.OnModelCreating(modelBuilder);

    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseModel>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserID ?? 0;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedBy = _currentUserService.UserID ?? 0;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Deleted:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.IsDeleted = true;
                    break;
                default:
                    break;
            }

        }

        return base.SaveChangesAsync(cancellationToken);
    }

    #region DbSets
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
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

    #endregion


}

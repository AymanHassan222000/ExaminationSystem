using ExaminationSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.ModelsConfigurations;

public class ExamAttemptConfigurations : IEntityTypeConfiguration<ExamAttempt>
{
    public void Configure(EntityTypeBuilder<ExamAttempt> builder)
    {
        builder.HasIndex(e => new { e.StudentID, e.ExamID }).IsUnique();

        builder.HasOne(m => m.Student)
               .WithMany(m => m.ExamAttempts)
               .HasForeignKey(m => m.StudentID)
               .HasPrincipalKey(m => m.UserID);

        builder.HasOne(m => m.Exam)
               .WithMany(m => m.ExamAttempts)
               .HasForeignKey(m => m.ExamID)
               .HasPrincipalKey(m => m.ID);
    }
}

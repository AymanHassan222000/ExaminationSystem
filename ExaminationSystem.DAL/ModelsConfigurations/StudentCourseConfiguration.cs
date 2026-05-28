using ExaminationSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.DAL.ModelsConfigurations;

public class StudentCourseConfiguration : IEntityTypeConfiguration<StudentCourse>
{
    public void Configure(EntityTypeBuilder<StudentCourse> builder)
    {
        builder.HasOne(sc => sc.Course)
               .WithMany(c => c.StudentCourses)
               .HasPrincipalKey(c => c.ID)
               .HasForeignKey(sc => sc.CourseID);

        builder.HasOne(sc => sc.Student)
               .WithMany(s => s.StudentCourses)
               .HasPrincipalKey(s => s.UserID)
               .HasForeignKey(sc => sc.StudetnID);
    }
}

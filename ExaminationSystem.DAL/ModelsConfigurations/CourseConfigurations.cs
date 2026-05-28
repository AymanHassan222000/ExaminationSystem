using ExaminationSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.ModelsConfigurations;

public class CourseConfigurations : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasOne(c => c.Instructor)
            .WithMany(i => i.Courses)
            .HasForeignKey(c => c.InstructorID)
            .HasPrincipalKey(i => i.UserID);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(150);
    }
}

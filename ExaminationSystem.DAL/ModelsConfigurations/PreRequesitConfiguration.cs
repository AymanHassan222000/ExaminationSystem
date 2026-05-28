using ExaminationSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.DAL.ModelsConfigurations
{
    public class PreRequesitConfiguration : IEntityTypeConfiguration<PreRequesit>
    {
        public void Configure(EntityTypeBuilder<PreRequesit> builder)
        {
            builder.HasIndex(p => new { p.PreRequisiteID, p.MainCourseID })
                   .IsUnique();
        }
    }
}

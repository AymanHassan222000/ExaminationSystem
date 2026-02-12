using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.ModelsConfigrations
{
    public class ExamQuestionConfigurations : IEntityTypeConfiguration<ExamQuestion>
    {
        public void Configure(EntityTypeBuilder<ExamQuestion> builder)
        {
            builder.HasIndex(e => new { e.ExamID, e.QuestionID }).IsUnique();
        }
    }
}

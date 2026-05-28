using ExaminationSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.ModelsConfigurations;

public class ChoiceConfigurations : IEntityTypeConfiguration<Choice>
{
    public void Configure(EntityTypeBuilder<Choice> builder)
    {
        builder.HasIndex(c => c.QuestionID)
               .IsUnique()
               .HasFilter("[IsCorrect] = 1");

    }
}

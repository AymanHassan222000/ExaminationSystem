using ExaminationSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.ModelsConfigurations;

public class RoleConfigurations : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasData(
            new Role { ID = 1, Name = "Admin" },
            new Role { ID = 2, Name = "Instructor" },
            new Role { ID = 3, Name = "Student" }
        );

    }
}

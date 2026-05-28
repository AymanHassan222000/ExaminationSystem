using ExaminationSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.ModelsConfigurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        //Email
        builder.HasIndex(u => u.Email)
               .IsUnique();

        builder.Property(u => u.Email)
               .IsRequired();

        //Phone Number
        builder.HasIndex(u => u.PhoneNumber)
               .IsUnique();

        builder.Property(u => u.PhoneNumber)
               .IsRequired()
               .HasMaxLength(12);

        //First Name
        builder.Property(u => u.FirstName)
               .IsRequired()
               .HasMaxLength(50);

        //Last Name
        builder.Property(u => u.LastName)
               .IsRequired()
               .HasMaxLength(50);

        //Password
        builder.Property(u => u.Password)
               .IsRequired();

        builder.HasOne(u => u.Instructor)
               .WithOne(i => i.User)
               .HasForeignKey<Instructor>(i => i.UserID);


    }
}

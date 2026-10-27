using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OmniReserve.Domain.Entities;
using OmniReserve.Domain.ValueObjects;

namespace OmniReserve.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(75);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(75);

        builder.Property(u => u.Email)
            .HasConversion(
                emailObj => emailObj.Value,             
                emailStr => new EmailAddress(emailStr)  
            )
            .HasColumnName("Email")
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.Password)
            .IsRequired();

        builder.Property(u => u.UserRole)
            .HasConversion<string>();
    }
}
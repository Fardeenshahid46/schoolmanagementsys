using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMS.Domain.Entities;

namespace SMS.Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Primary Key
        builder.HasKey(u => u.Id);

        // Required fields
        builder.Property(u => u.FirstName)
            .IsRequired();

        builder.Property(u => u.LastName)
            .IsRequired();

        builder.Property(u => u.Email)
            .IsRequired();

        // Unique index on Email
        builder.HasIndex(u => u.Email)
            .IsUnique();

        // PasswordHash
        builder.Property(u => u.PasswordHash)
            .IsRequired();

        // Role
        builder.Property(u => u.Role)
            .IsRequired()
            .HasMaxLength(50);

        // Relationship: Tenant -> many ApplicationUsers
        builder.HasOne(u => u.Tenant)
            .WithMany(t => t.ApplicationUsers)
            .HasForeignKey(u => u.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship: ApplicationUser -> many RefreshTokens
        builder.HasMany(u => u.RefreshTokens)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

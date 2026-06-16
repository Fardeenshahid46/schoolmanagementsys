using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMS.Domain.Entities;

namespace SMS.Persistence.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        // Primary Key
        builder.HasKey(t => t.Id);

        // Name
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

        // Subdomain
        builder.Property(t => t.Subdomain)
            .IsRequired()
            .HasMaxLength(100);

        // Unique index on Subdomain
        builder.HasIndex(t => t.Subdomain)
            .IsUnique();

        // Relationship: Tenant -> many ApplicationUsers
        builder.HasMany(t => t.ApplicationUsers)
            .WithOne(u => u.Tenant)
            .HasForeignKey(u => u.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

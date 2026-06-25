using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMS.Domain.Entities;

namespace SMS.Persistence.Configurations;

public class SchoolClassConfiguration
    : IEntityTypeConfiguration<SchoolClass>
{
    public void Configure(
        EntityTypeBuilder<SchoolClass> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Section)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(x => x.Tenant)
            .WithMany(x => x.Classes)
            .HasForeignKey(x => x.TenantId);
    }
}
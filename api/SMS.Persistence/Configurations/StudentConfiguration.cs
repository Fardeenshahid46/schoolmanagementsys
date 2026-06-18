using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMS.Domain.Entities;

namespace SMS.Persistence.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Gender)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(s => s.AdmissionNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(s => new
        {
            s.TenantId,
            s.AdmissionNumber
        }).IsUnique();

        builder.Property(s => s.GuardianName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(s => s.GuardianPhone)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(s => s.Tenant)
            .WithMany(t => t.Students)
            .HasForeignKey(s => s.TenantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
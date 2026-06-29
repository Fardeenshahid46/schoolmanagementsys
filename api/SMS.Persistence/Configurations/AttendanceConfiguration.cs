using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMS.Domain.Entities;

namespace SMS.Persistence.Configurations;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.ToTable("Attendances");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TenantId)
            .IsRequired();

        builder.Property(x => x.StudentId)
            .IsRequired();

        builder.Property(x => x.ClassId)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.AttendanceDate)
            .IsRequired();

        builder.HasOne(x => x.Tenant)
            .WithMany()
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Student)
            .WithMany()
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.SchoolClass)
            .WithMany()
            .HasForeignKey(x => x.ClassId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Property(x => x.IsActive).IsRequired();

        builder.Property(x => x.Remarks).HasMaxLength(250);    
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SMS.Domain.Entities;

namespace SMS.Persistence.Configurations;

public class StudentClassConfiguration : IEntityTypeConfiguration<StudentClass>
{
    public void Configure(EntityTypeBuilder<StudentClass> builder)
    {
        builder.HasKey(x => x.Id);

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
    }
}

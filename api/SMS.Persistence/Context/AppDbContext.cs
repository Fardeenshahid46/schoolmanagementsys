using Microsoft.EntityFrameworkCore;
using SMS.Domain.Entities;

namespace SMS.Persistence.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<SchoolClass> Classes => Set<SchoolClass>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<TeacherSubject> TeacherSubjects => Set<TeacherSubject>();
    public DbSet<StudentClass> StudentClasses => Set<StudentClass>();
    public DbSet<Attendance> Attendances => Set<Attendance>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}

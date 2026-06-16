using Microsoft.EntityFrameworkCore;
using SMS.Domain.Entities;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Persistence.Seed;

public class DatabaseSeeder
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public DatabaseSeeder(AppDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync()
    {
        // Seed default Tenant
        if (!await _context.Tenants.AnyAsync())
        {
            var tenant = new Tenant
            {
                Id = Guid.NewGuid(),
                Name = "Demo School",
                Subdomain = "demo",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Tenants.Add(tenant);
            await _context.SaveChangesAsync();
        }

        // Seed default Admin user
        if (!await _context.ApplicationUsers.AnyAsync(u => u.Email == "admin@school.com"))
        {
            var tenant = await _context.Tenants.FirstAsync();

            var adminUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                TenantId = tenant.Id,
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@school.com",
                PasswordHash = _passwordHasher.HashPassword("123456"),
                Role = "Admin",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.ApplicationUsers.Add(adminUser);
            await _context.SaveChangesAsync();
        }
    }
}

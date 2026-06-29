using Microsoft.EntityFrameworkCore;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.StudentClasses.GetStudentClasses;

public class GetStudentClassesHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public GetStudentClassesHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<List<StudentClassListItemDto>> HandleAsync(
        Guid studentId,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }

        var tenantExists = await _dbContext.Tenants
            .AnyAsync(
                t => t.Id == tenantId.Value && t.IsActive,
                cancellationToken);

        if (!tenantExists)
        {
            throw new InvalidOperationException(
                "Tenant does not exist.");
        }

        return await _dbContext.StudentClasses
            .Include(sc => sc.Student)
            .Include(sc => sc.SchoolClass)
            .Where(sc =>
                sc.StudentId == studentId &&
                sc.TenantId == tenantId.Value &&
                sc.IsActive)
            .Select(sc => new StudentClassListItemDto
            {
                Id = sc.Id,
                StudentId = sc.StudentId,
                StudentName = sc.Student.FirstName + " " + sc.Student.LastName,
                ClassId = sc.ClassId,
                ClassName = sc.SchoolClass.Name + " - " + sc.SchoolClass.Section,
                IsActive = sc.IsActive
            })
            .ToListAsync(cancellationToken);
    }
}

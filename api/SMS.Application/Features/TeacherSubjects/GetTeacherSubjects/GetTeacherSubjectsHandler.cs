using Microsoft.EntityFrameworkCore;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.TeacherSubjects.GetTeacherSubjects;

public class GetTeacherSubjectsHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public GetTeacherSubjectsHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<List<TeacherSubjectListItemDto>> HandleAsync(
        Guid teacherId,
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

        return await _dbContext.TeacherSubjects
            .Include(ts => ts.Teacher)
            .Include(ts => ts.Subject)
            .Where(ts =>
                ts.TeacherId == teacherId &&
                ts.TenantId == tenantId.Value &&
                ts.IsActive)
            .Select(ts => new TeacherSubjectListItemDto
            {
                Id = ts.Id,
                TeacherId = ts.TeacherId,
                TeacherName = ts.Teacher.FirstName + " " + ts.Teacher.LastName,
                SubjectId = ts.SubjectId,
                SubjectName = ts.Subject.Name,
                IsActive = ts.IsActive
            })
            .ToListAsync(cancellationToken);
    }
}

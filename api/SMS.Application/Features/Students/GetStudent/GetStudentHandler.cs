using Microsoft.EntityFrameworkCore;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Students.GetStudent;

public class GetStudentHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public GetStudentHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<List<StudentListItemDto>> HandleAsync(
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }

        return await _dbContext.Students
            .Where(s => s.TenantId == tenantId.Value)
            .Where(s => s.IsActive)
            .Select(s => new StudentListItemDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                AdmissionNumber = s.AdmissionNumber,
                GuardianName = s.GuardianName,
                GuardianPhone = s.GuardianPhone
            })
            .ToListAsync(cancellationToken);
    }
}
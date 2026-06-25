using Microsoft.EntityFrameworkCore;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Subjects.GetSubject;

public class GetSubjectHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public GetSubjectHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<List<SubjectListItemDto>> HandleAsync(
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }

        return await _dbContext.Subjects
            .Where(s =>
                s.TenantId == tenantId.Value &&
                s.IsActive)
            .OrderBy(s => s.Name)
            .Select(s => new SubjectListItemDto
            {
                Id = s.Id,
                Name = s.Name,
                Code = s.Code
            })
            .ToListAsync(cancellationToken);
    }
}

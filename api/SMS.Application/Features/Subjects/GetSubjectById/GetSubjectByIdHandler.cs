using Microsoft.EntityFrameworkCore;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Subjects.GetSubjectById;

public class GetSubjectByIdHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public GetSubjectByIdHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<SubjectDetailsDto?> HandleAsync(
        Guid id,
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
                s.Id == id &&
                s.TenantId == tenantId.Value &&
                s.IsActive)
            .Select(s => new SubjectDetailsDto
            {
                Id = s.Id,
                Name = s.Name,
                Code = s.Code,
                IsActive = s.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}

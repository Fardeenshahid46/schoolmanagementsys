using Microsoft.EntityFrameworkCore;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Classes.GetClass;

public class GetClassHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public GetClassHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<List<ClassListItemDto>> HandleAsync(
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }

        return await _dbContext.Classes
            .Where(c =>
                c.TenantId == tenantId.Value &&
                c.IsActive)
            .OrderBy(c => c.Name)
            .Select(c => new ClassListItemDto
            {
                Id = c.Id,
                Name = c.Name,
                Section = c.Section
            })
            .ToListAsync(cancellationToken);
    }
}
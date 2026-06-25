using Microsoft.EntityFrameworkCore;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Classes.GetClassById;

public class GetClassByIdHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public GetClassByIdHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<ClassDetailsDto?> HandleAsync(
        Guid id,
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
                c.Id == id &&
                c.TenantId == tenantId.Value &&
                c.IsActive)
            .Select(c => new ClassDetailsDto
            {
                Id = c.Id,
                Name = c.Name,
                Section = c.Section
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
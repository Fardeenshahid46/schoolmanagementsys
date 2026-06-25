using Microsoft.EntityFrameworkCore;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Classes.DeleteClass;

public class DeleteClassHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public DeleteClassHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<bool> HandleAsync(
        DeleteClassCommand command,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }

        var schoolClass = await _dbContext.Classes
            .FirstOrDefaultAsync(
                c => c.Id == command.Id
                  && c.TenantId == tenantId.Value
                  && c.IsActive,
                cancellationToken);

        if (schoolClass == null)
        {
            return false;
        }

        schoolClass.IsActive = false;

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return true;
    }
}
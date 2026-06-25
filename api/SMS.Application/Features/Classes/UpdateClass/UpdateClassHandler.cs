using Microsoft.EntityFrameworkCore;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Classes.UpdateClass;

public class UpdateClassHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public UpdateClassHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<bool> HandleAsync(
        UpdateClassCommand command,
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

        schoolClass.Name = command.Name;
        schoolClass.Section = command.Section;

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return true;
    }
}
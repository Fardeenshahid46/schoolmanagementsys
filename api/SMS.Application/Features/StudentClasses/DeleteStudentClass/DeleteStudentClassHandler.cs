using Microsoft.EntityFrameworkCore;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.StudentClasses.DeleteStudentClass;

public class DeleteStudentClassHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public DeleteStudentClassHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<bool> HandleAsync(
        DeleteStudentClassCommand command,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }

        var studentClass = await _dbContext.StudentClasses
            .FirstOrDefaultAsync(
                sc => sc.Id == command.Id
                   && sc.TenantId == tenantId.Value
                   && sc.IsActive,
                cancellationToken);

        if (studentClass == null)
        {
            return false;
        }

        studentClass.IsActive = false;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}

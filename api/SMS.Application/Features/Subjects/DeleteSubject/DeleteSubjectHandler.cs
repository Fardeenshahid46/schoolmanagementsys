using Microsoft.EntityFrameworkCore;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Subjects.DeleteSubject;

public class DeleteSubjectHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public DeleteSubjectHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<bool> HandleAsync(
        DeleteSubjectCommand command,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }

        var subject = await _dbContext.Subjects
            .FirstOrDefaultAsync(
                s => s.Id == command.Id
                  && s.TenantId == tenantId.Value
                  && s.IsActive,
                cancellationToken);

        if (subject == null)
        {
            return false;
        }

        subject.IsActive = false;

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return true;
    }
}

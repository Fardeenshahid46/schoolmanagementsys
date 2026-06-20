using Microsoft.EntityFrameworkCore;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Teachers.DeleteTeacher;

public class DeleteTeacherHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public DeleteTeacherHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<bool> HandleAsync(
        DeleteTeacherCommand command,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }

        var teacher = await _dbContext.Teachers
            .FirstOrDefaultAsync(
                t =>
                t.Id == command.Id &&
                t.TenantId == tenantId.Value &&
                t.IsActive,
                cancellationToken);

        if (teacher == null)
        {
            return false;
        }

        teacher.IsActive = false;

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return true;
    }
}
using Microsoft.EntityFrameworkCore;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Attendance.DeleteAttendance;

public class DeleteAttendanceHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public DeleteAttendanceHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<bool> HandleAsync(
        DeleteAttendanceCommand command,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException("Tenant not resolved.");
        }

        var tenantExists = await _dbContext.Tenants
            .AnyAsync(
                t => t.Id == tenantId.Value && t.IsActive,
                cancellationToken);

        if (!tenantExists)
        {
            throw new InvalidOperationException("Tenant does not exist.");
        }

        var attendance = await _dbContext.Attendances
            .FirstOrDefaultAsync(
                a => a.Id == command.Id &&
                     a.TenantId == tenantId.Value &&
                     a.IsActive,
                cancellationToken);

        if (attendance == null)
        {
            return false;
        }

        attendance.IsActive = false;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}

using Microsoft.EntityFrameworkCore;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Teachers.UpdateTeacher;

public class UpdateTeacherHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public UpdateTeacherHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<bool> HandleAsync(
        UpdateTeacherCommand command,
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

        teacher.FirstName = command.FirstName;
        teacher.LastName = command.LastName;
        teacher.Email = command.Email;
        teacher.PhoneNumber = command.PhoneNumber;
        teacher.Qualification = command.Qualification;
        teacher.Address = command.Address;

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return true;
    }
}
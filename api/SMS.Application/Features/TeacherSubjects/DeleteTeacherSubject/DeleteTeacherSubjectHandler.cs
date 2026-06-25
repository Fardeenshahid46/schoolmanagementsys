using Microsoft.EntityFrameworkCore;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.TeacherSubjects.DeleteTeacherSubject;

public class DeleteTeacherSubjectHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public DeleteTeacherSubjectHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<bool> HandleAsync(
        DeleteTeacherSubjectCommand command,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }

        var teacherSubject = await _dbContext.TeacherSubjects
            .FirstOrDefaultAsync(
                ts => ts.Id == command.Id
                   && ts.TenantId == tenantId.Value
                   && ts.IsActive,
                cancellationToken);

        if (teacherSubject == null)
        {
            return false;
        }

        teacherSubject.IsActive = false;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}

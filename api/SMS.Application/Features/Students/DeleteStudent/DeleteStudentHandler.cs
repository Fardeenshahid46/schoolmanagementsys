using Microsoft.EntityFrameworkCore;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Students.DeleteStudent;

public class DeleteStudentHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;


    public DeleteStudentHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }


    public async Task<bool> HandleAsync(
        DeleteStudentCommand command,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }


        var student = await _dbContext.Students
            .FirstOrDefaultAsync(
                s =>
                s.Id == command.Id &&
                s.TenantId == tenantId.Value &&
                s.IsActive,
                cancellationToken);


        if (student == null)
        {
            return false;
        }


        student.IsActive = false;


        await _dbContext.SaveChangesAsync(
            cancellationToken);


        return true;
    }
}
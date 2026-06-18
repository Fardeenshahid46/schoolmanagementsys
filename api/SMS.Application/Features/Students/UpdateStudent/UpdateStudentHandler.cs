using Microsoft.EntityFrameworkCore;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Students.UpdateStudent;

public class UpdateStudentHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;


    public UpdateStudentHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }


    public async Task<bool> HandleAsync(
        UpdateStudentCommand command,
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


        student.FirstName = command.FirstName;
        student.LastName = command.LastName;
        student.Gender = command.Gender;
        student.DateOfBirth = command.DateOfBirth;
        student.GuardianName = command.GuardianName;
        student.GuardianPhone = command.GuardianPhone;
        student.Address = command.Address;


        await _dbContext.SaveChangesAsync(
            cancellationToken);


        return true;
    }
}
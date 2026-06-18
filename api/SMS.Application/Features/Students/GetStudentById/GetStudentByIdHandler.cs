using Microsoft.EntityFrameworkCore;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Students.GetStudentById;


public class GetStudentByIdHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;


    public GetStudentByIdHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }


    public async Task<StudentDetailsDto?> HandleAsync(
        Guid id,
        CancellationToken cancellationToken)
    {

        var tenantId = _tenantProvider.TenantId;


        if(!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }


        return await _dbContext.Students

            .Where(s =>
                s.Id == id &&
                s.TenantId == tenantId.Value &&
                s.IsActive)

            .Select(s => new StudentDetailsDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Gender = s.Gender,
                DateOfBirth = s.DateOfBirth,
                AdmissionNumber = s.AdmissionNumber,
                GuardianName = s.GuardianName,
                GuardianPhone = s.GuardianPhone,
                Address = s.Address
            })

            .FirstOrDefaultAsync(cancellationToken);
    }
}
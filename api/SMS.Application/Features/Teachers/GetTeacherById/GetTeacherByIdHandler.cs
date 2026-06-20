using Microsoft.EntityFrameworkCore;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Teachers.GetTeacherById;

public class GetTeacherByIdHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public GetTeacherByIdHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<TeacherDetailsDto?> HandleAsync(
        Guid teacherId,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }

        return await _dbContext.Teachers
            .Where(t =>
                t.Id == teacherId &&
                t.TenantId == tenantId.Value &&
                t.IsActive)
            .Select(t => new TeacherDetailsDto
            {
                Id = t.Id,
                FirstName = t.FirstName,
                LastName = t.LastName,
                Email = t.Email,
                PhoneNumber = t.PhoneNumber,
                Qualification = t.Qualification,
                Address = t.Address
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
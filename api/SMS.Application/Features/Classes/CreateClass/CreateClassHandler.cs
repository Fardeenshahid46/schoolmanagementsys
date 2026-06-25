using SMS.Application.DTOs;
using SMS.Domain.Entities;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Classes.CreateClass;

public class CreateClassHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public CreateClassHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<ClassResponseDto> HandleAsync(
        CreateClassCommand command,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }

        var schoolClass = new SchoolClass
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId.Value,
            Name = command.Name,
            Section = command.Section,
            IsActive = true
        };

        _dbContext.Classes.Add(schoolClass);

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return new ClassResponseDto
        {
            Id = schoolClass.Id,
            Name = schoolClass.Name,
            Section = schoolClass.Section
        };
    }
}
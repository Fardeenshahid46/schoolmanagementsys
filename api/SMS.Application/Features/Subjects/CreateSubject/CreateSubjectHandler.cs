using SMS.Application.DTOs;
using SMS.Domain.Entities;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Subjects.CreateSubject;

public class CreateSubjectHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public CreateSubjectHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<SubjectResponseDto> HandleAsync(
        CreateSubjectCommand command,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }

        var subject = new Subject
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId.Value,
            Name = command.Name,
            Code = command.Code,
            IsActive = true
        };

        _dbContext.Subjects.Add(subject);

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return new SubjectResponseDto
        {
            Id = subject.Id,
            Name = subject.Name,
            Code = subject.Code,
            IsActive = subject.IsActive
        };
    }
}

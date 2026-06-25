using Microsoft.EntityFrameworkCore;
using SMS.Application.DTOs;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Subjects.UpdateSubject;

public class UpdateSubjectHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public UpdateSubjectHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<SubjectResponseDto?> HandleAsync(
        UpdateSubjectCommand command,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }

        var subject = await _dbContext.Subjects
            .FirstOrDefaultAsync(
                s => s.Id == command.Id
                  && s.TenantId == tenantId.Value
                  && s.IsActive,
                cancellationToken);

        if (subject == null)
        {
            return null;
        }

        subject.Name = command.Name;
        subject.Code = command.Code;

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

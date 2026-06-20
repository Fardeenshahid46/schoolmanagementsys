using SMS.Application.DTOs;
using SMS.Domain.Entities;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Teachers.CreateTeacher;

public class CreateTeacherHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public CreateTeacherHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<TeacherResponseDto> HandleAsync(
        CreateTeacherCommand command,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }

        var teacher = new Teacher
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId.Value,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber,
            Qualification = command.Qualification,
            Address = command.Address,
            IsActive = true
        };

        _dbContext.Teachers.Add(teacher);

        await _dbContext.SaveChangesAsync(
            cancellationToken);

        return new TeacherResponseDto
        {
            Id = teacher.Id,
            FirstName = teacher.FirstName,
            LastName = teacher.LastName,
            Email = teacher.Email
        };
    }
}
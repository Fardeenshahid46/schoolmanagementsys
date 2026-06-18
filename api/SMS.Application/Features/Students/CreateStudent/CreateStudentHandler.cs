using SMS.Application.DTOs;
using SMS.Domain.Entities;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace SMS.Application.Features.Students.CreateStudent;

public class CreateStudentHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public CreateStudentHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<StudentResponseDto> HandleAsync(
        CreateStudentCommand command,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

if (!tenantId.HasValue)
{
    throw new InvalidOperationException("Tenant not resolved.");
}

        var exists = await _dbContext.Students.AnyAsync(
            s => s.TenantId == tenantId.Value
              && s.AdmissionNumber == command.AdmissionNumber,
            cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException(
                "Admission number already exists.");
        }

        var student = new Student
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId.Value,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Gender = command.Gender,
            DateOfBirth = command.DateOfBirth,
            AdmissionNumber = command.AdmissionNumber,
            GuardianName = command.GuardianName,
            GuardianPhone = command.GuardianPhone,
            Address = command.Address,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _dbContext.Students.Add(student);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new StudentResponseDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            AdmissionNumber = student.AdmissionNumber,
            GuardianName = student.GuardianName,
            GuardianPhone = student.GuardianPhone
        };
    }
}
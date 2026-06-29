using Microsoft.EntityFrameworkCore;
using SMS.Application.DTOs;
using SMS.Domain.Entities;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.StudentClasses.CreateStudentClass;

public class CreateStudentClassHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public CreateStudentClassHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<StudentClassResponseDto> HandleAsync(
        CreateStudentClassCommand command,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }

        var tenantExists = await _dbContext.Tenants
            .AnyAsync(
                t => t.Id == tenantId.Value && t.IsActive,
                cancellationToken);

        if (!tenantExists)
        {
            throw new InvalidOperationException(
                "Tenant does not exist.");
        }

        var studentExists = await _dbContext.Students
            .AnyAsync(
                s => s.Id == command.StudentId &&
                     s.TenantId == tenantId.Value &&
                     s.IsActive,
                cancellationToken);

        if (!studentExists)
        {
            throw new InvalidOperationException(
                "Student not found for this tenant.");
        }

        var classExists = await _dbContext.Classes
            .AnyAsync(
                c => c.Id == command.ClassId &&
                     c.TenantId == tenantId.Value &&
                     c.IsActive,
                cancellationToken);

        if (!classExists)
        {
            throw new InvalidOperationException(
                "Class not found for this tenant.");
        }

        var duplicateExists = await _dbContext.StudentClasses
            .AnyAsync(
                sc => sc.StudentId == command.StudentId &&
                      sc.ClassId == command.ClassId &&
                      sc.TenantId == tenantId.Value &&
                      sc.IsActive,
                cancellationToken);

        if (duplicateExists)
        {
            throw new InvalidOperationException(
                "This student is already assigned to the class.");
        }

        var studentClass = new StudentClass
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId.Value,
            StudentId = command.StudentId,
            ClassId = command.ClassId,
            IsActive = true
        };

        _dbContext.StudentClasses.Add(studentClass);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new StudentClassResponseDto
        {
            Id = studentClass.Id,
            StudentId = studentClass.StudentId,
            ClassId = studentClass.ClassId,
            IsActive = studentClass.IsActive
        };
    }
}

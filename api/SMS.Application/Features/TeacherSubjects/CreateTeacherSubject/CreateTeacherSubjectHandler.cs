using Microsoft.EntityFrameworkCore;
using SMS.Application.DTOs;
using SMS.Domain.Entities;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.TeacherSubjects.CreateTeacherSubject;

public class CreateTeacherSubjectHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public CreateTeacherSubjectHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<TeacherSubjectResponseDto> HandleAsync(
        CreateTeacherSubjectCommand command,
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

        var teacherExists = await _dbContext.Teachers
            .AnyAsync(
                t => t.Id == command.TeacherId &&
                     t.TenantId == tenantId.Value &&
                     t.IsActive,
                cancellationToken);

        if (!teacherExists)
        {
            throw new InvalidOperationException(
                "Teacher not found for this tenant.");
        }

        var subjectExists = await _dbContext.Subjects
            .AnyAsync(
                s => s.Id == command.SubjectId &&
                     s.TenantId == tenantId.Value &&
                     s.IsActive,
                cancellationToken);

        if (!subjectExists)
        {
            throw new InvalidOperationException(
                "Subject not found for this tenant.");
        }

        var duplicateExists = await _dbContext.TeacherSubjects
            .AnyAsync(
                ts => ts.TeacherId == command.TeacherId &&
                      ts.SubjectId == command.SubjectId &&
                      ts.TenantId == tenantId.Value &&
                      ts.IsActive,
                cancellationToken);

        if (duplicateExists)
        {
            throw new InvalidOperationException(
                "This subject is already assigned to the teacher.");
        }

        var teacherSubject = new TeacherSubject
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId.Value,
            TeacherId = command.TeacherId,
            SubjectId = command.SubjectId,
            IsActive = true
        };

        _dbContext.TeacherSubjects.Add(teacherSubject);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new TeacherSubjectResponseDto
        {
            Id = teacherSubject.Id,
            TeacherId = teacherSubject.TeacherId,
            SubjectId = teacherSubject.SubjectId,
            IsActive = teacherSubject.IsActive
        };
    }
}

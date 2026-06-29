using Microsoft.EntityFrameworkCore;
using SMS.Application.DTOs;
using SMS.Domain.Entities;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Attendance.CreateAttendance;

public class CreateAttendanceHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public CreateAttendanceHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<AttendanceResponseDto> HandleAsync(
        CreateAttendanceCommand command,
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

        var duplicateExists = await _dbContext.Attendances
            .AnyAsync(
                a => a.StudentId == command.StudentId &&
                     a.ClassId == command.ClassId &&
                     a.AttendanceDate == command.AttendanceDate &&
                     a.TenantId == tenantId.Value &&
                     a.IsActive,
                cancellationToken);

        if (duplicateExists)
        {
            throw new InvalidOperationException(
                "Attendance already exists.");
        }

        var attendance = new SMS.Domain.Entities.Attendance
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId.Value,
            StudentId = command.StudentId,
            ClassId = command.ClassId,
            Status = command.Status,
            AttendanceDate = command.AttendanceDate,
            Remarks = command.Remarks,
            IsActive = true
        };

        _dbContext.Attendances.Add(attendance);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new AttendanceResponseDto
        {
            Id = attendance.Id,
            TenantId = attendance.TenantId,
            StudentId = attendance.StudentId,
            ClassId = attendance.ClassId,
            Status = attendance.Status,
            AttendanceDate = attendance.AttendanceDate,
            Remarks = attendance.Remarks,
            IsActive = attendance.IsActive
        };
    }
}

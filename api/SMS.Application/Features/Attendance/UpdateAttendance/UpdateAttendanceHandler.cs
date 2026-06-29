using Microsoft.EntityFrameworkCore;
using SMS.Application.DTOs;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Attendance.UpdateAttendance;

public class UpdateAttendanceHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public UpdateAttendanceHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<AttendanceResponseDto> HandleAsync(
        UpdateAttendanceCommand command,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException("Tenant not resolved.");
        }

        var tenantExists = await _dbContext.Tenants
            .AnyAsync(
                t => t.Id == tenantId.Value && t.IsActive,
                cancellationToken);

        if (!tenantExists)
        {
            throw new InvalidOperationException("Tenant does not exist.");
        }

        // Validate Status
        var allowedStatuses = new[] { "Present", "Absent", "Late", "Leave" };
        if (string.IsNullOrWhiteSpace(command.Status) || 
            !allowedStatuses.Contains(command.Status, StringComparer.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Invalid status. Supported values are: Present, Absent, Late, Leave.");
        }

        var attendance = await _dbContext.Attendances
            .FirstOrDefaultAsync(
                a => a.Id == command.Id &&
                     a.TenantId == tenantId.Value &&
                     a.IsActive,
                cancellationToken);

        if (attendance == null)
        {
            throw new InvalidOperationException("Attendance not found.");
        }

        attendance.Status = command.Status;
        attendance.AttendanceDate = command.AttendanceDate;
        attendance.Remarks = command.Remarks;

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

using Microsoft.EntityFrameworkCore;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Attendance.GetAttendanceById;

public class GetAttendanceByIdHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public GetAttendanceByIdHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<AttendanceDetailsDto?> HandleAsync(
        Guid id,
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

        return await _dbContext.Attendances
            .Include(a => a.Student)
            .Include(a => a.SchoolClass)
            .Where(a =>
                a.Id == id &&
                a.TenantId == tenantId.Value &&
                a.IsActive)
            .Select(a => new AttendanceDetailsDto
            {
                Id = a.Id,
                StudentId = a.StudentId,
                StudentName = a.Student.FirstName + " " + a.Student.LastName,
                ClassId = a.ClassId,
                ClassName = a.SchoolClass.Name + " - " + a.SchoolClass.Section,
                Status = a.Status,
                AttendanceDate = a.AttendanceDate,
                Remarks = a.Remarks,
                IsActive = a.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}

using Microsoft.EntityFrameworkCore;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Attendance.GetAttendance;

public class GetAttendanceHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public GetAttendanceHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<List<AttendanceListItemDto>> HandleAsync(
        GetAttendanceQuery query,
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

        var dbQuery = _dbContext.Attendances
            .Include(a => a.Student)
            .Include(a => a.SchoolClass)
            .Where(a => a.TenantId == tenantId.Value && a.IsActive);

        if (query.StudentId.HasValue)
        {
            dbQuery = dbQuery.Where(a => a.StudentId == query.StudentId.Value);
        }

        if (query.ClassId.HasValue)
        {
            dbQuery = dbQuery.Where(a => a.ClassId == query.ClassId.Value);
        }

        return await dbQuery
            .OrderByDescending(a => a.AttendanceDate)
            .Select(a => new AttendanceListItemDto
            {
                Id = a.Id,
                StudentId = a.StudentId,
                StudentName = a.Student.FirstName + " " + a.Student.LastName,
                ClassId = a.ClassId,
                ClassName = a.SchoolClass.Name + " - " + a.SchoolClass.Section,
                Status = a.Status,
                AttendanceDate = a.AttendanceDate,
                Remarks = a.Remarks
            })
            .ToListAsync(cancellationToken);
    }
}

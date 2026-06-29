using Microsoft.EntityFrameworkCore;
using SMS.Application.Features.Attendance.GetAttendance;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Attendance.SearchAttendance;

public class SearchAttendanceHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public SearchAttendanceHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<PagedAttendanceResponseDto> HandleAsync(
        SearchAttendanceQuery query,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }

        var dbQuery = _dbContext.Attendances
            .Include(a => a.Student)
            .Include(a => a.SchoolClass)
            .Where(a => a.TenantId == tenantId.Value && a.IsActive);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            dbQuery = dbQuery.Where(a =>
                a.Student.FirstName.Contains(query.Search) ||
                a.Student.LastName.Contains(query.Search) ||
                a.Student.AdmissionNumber.Contains(query.Search) ||
                a.SchoolClass.Name.Contains(query.Search) ||
                a.SchoolClass.Section.Contains(query.Search) ||
                a.Status.Contains(query.Search));
        }

        if (query.StudentId.HasValue)
        {
            dbQuery = dbQuery.Where(a => a.StudentId == query.StudentId.Value);
        }

        if (query.ClassId.HasValue)
        {
            dbQuery = dbQuery.Where(a => a.ClassId == query.ClassId.Value);
        }

        if (query.Date.HasValue)
        {
            dbQuery = dbQuery.Where(a => a.AttendanceDate.Date == query.Date.Value.Date);
        }

        var totalCount = await dbQuery.CountAsync(cancellationToken);

        var items = await dbQuery
            .OrderByDescending(a => a.AttendanceDate)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
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

        return new PagedAttendanceResponseDto
        {
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            Items = items
        };
    }
}

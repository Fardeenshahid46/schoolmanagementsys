using Microsoft.EntityFrameworkCore;
using SMS.Application.Features.StudentClasses.GetStudentClasses;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.StudentClasses.SearchStudentClasses;

public class SearchStudentClassHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public SearchStudentClassHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<PagedStudentClassResponseDto> HandleAsync(
        GetStudentClassQuery query,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }

        var studentClassesQuery = _dbContext.StudentClasses
            .Include(sc => sc.Student)
            .Include(sc => sc.SchoolClass)
            .Where(sc => sc.TenantId == tenantId.Value)
            .Where(sc => sc.IsActive);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            studentClassesQuery = studentClassesQuery.Where(sc =>
                sc.Student.FirstName.Contains(query.Search) ||
                sc.Student.LastName.Contains(query.Search) ||
                sc.Student.AdmissionNumber.Contains(query.Search) ||
                sc.SchoolClass.Name.Contains(query.Search) ||
                sc.SchoolClass.Section.Contains(query.Search));
        }

        var totalCount = await studentClassesQuery.CountAsync(
            cancellationToken);

        var items = await studentClassesQuery
            .OrderBy(sc => sc.Student.FirstName)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(sc => new StudentClassListItemDto
            {
                Id = sc.Id,
                StudentId = sc.StudentId,
                StudentName = sc.Student.FirstName + " " + sc.Student.LastName,
                ClassId = sc.ClassId,
                ClassName = sc.SchoolClass.Name + " - " + sc.SchoolClass.Section,
                IsActive = sc.IsActive
            })
            .ToListAsync(cancellationToken);

        return new PagedStudentClassResponseDto
        {
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            Items = items
        };
    }
}

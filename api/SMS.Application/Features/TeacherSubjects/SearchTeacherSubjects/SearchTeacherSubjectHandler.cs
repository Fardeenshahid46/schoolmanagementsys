using Microsoft.EntityFrameworkCore;
using SMS.Application.Features.TeacherSubjects.GetTeacherSubjects;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.TeacherSubjects.SearchTeacherSubjects;

public class SearchTeacherSubjectHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public SearchTeacherSubjectHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<PagedTeacherSubjectResponseDto> HandleAsync(
        GetTeacherSubjectQuery query,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }

        var teacherSubjectsQuery = _dbContext.TeacherSubjects
            .Include(ts => ts.Teacher)
            .Include(ts => ts.Subject)
            .Where(ts => ts.TenantId == tenantId.Value)
            .Where(ts => ts.IsActive);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            teacherSubjectsQuery = teacherSubjectsQuery.Where(ts =>
                ts.Teacher.FirstName.Contains(query.Search) ||
                ts.Teacher.LastName.Contains(query.Search) ||
                ts.Subject.Name.Contains(query.Search));
        }

        var totalCount = await teacherSubjectsQuery.CountAsync(cancellationToken);

        var items = await teacherSubjectsQuery
            .OrderBy(ts => ts.Teacher.FirstName)
            .ThenBy(ts => ts.Teacher.LastName)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(ts => new TeacherSubjectListItemDto
            {
                Id = ts.Id,
                TeacherId = ts.TeacherId,
                TeacherName = ts.Teacher.FirstName + " " + ts.Teacher.LastName,
                SubjectId = ts.SubjectId,
                SubjectName = ts.Subject.Name,
                IsActive = ts.IsActive
            })
            .ToListAsync(cancellationToken);

        return new PagedTeacherSubjectResponseDto
        {
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            Items = items
        };
    }
}

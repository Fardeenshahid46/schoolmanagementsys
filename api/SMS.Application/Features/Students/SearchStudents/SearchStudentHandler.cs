using SMS.Application.Features.Students.GetStudent;
using Microsoft.EntityFrameworkCore;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Students.SearchStudents;

public class SearchStudentHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public SearchStudentHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<PagedStudentResponseDto> HandleAsync(
        GetStudentQuery query,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }

        var studentsQuery = _dbContext.Students
            .Where(s => s.TenantId == tenantId.Value)
            .Where(s => s.IsActive);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            studentsQuery = studentsQuery.Where(s =>
                s.FirstName.Contains(query.Search) ||
                s.LastName.Contains(query.Search) ||
                s.AdmissionNumber.Contains(query.Search));
        }

        var totalCount = await studentsQuery.CountAsync(
            cancellationToken);

        var students = await studentsQuery
            .OrderBy(s => s.FirstName)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(s => new StudentListItemDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                AdmissionNumber = s.AdmissionNumber,
                GuardianName = s.GuardianName,
                GuardianPhone = s.GuardianPhone
            })
            .ToListAsync(cancellationToken);

        return new PagedStudentResponseDto
        {
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            Items = students
        };
    }
}
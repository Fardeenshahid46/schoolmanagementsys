using Microsoft.EntityFrameworkCore;
using SMS.Application.DTOs;
using SMS.Application.Features.Teachers.GetTeacher;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Teachers.SearchTeachers;

public class SearchTeacherHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public SearchTeacherHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<PagedTeacherResponseDto> HandleAsync(
        GetTeacherQuery query,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }

        var teachersQuery = _dbContext.Teachers
            .Where(t => t.TenantId == tenantId.Value)
            .Where(t => t.IsActive);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            teachersQuery = teachersQuery.Where(t =>
                t.FirstName.Contains(query.Search) ||
                t.LastName.Contains(query.Search) ||
                t.Email.Contains(query.Search));
        }

        var totalCount = await teachersQuery.CountAsync(
            cancellationToken);

        var teachers = await teachersQuery
            .OrderBy(t => t.FirstName)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(t => new TeacherListItemDto
            {
                Id = t.Id,
                FirstName = t.FirstName,
                LastName = t.LastName,
                Email = t.Email,
                PhoneNumber = t.PhoneNumber
            })
            .ToListAsync(cancellationToken);

        return new PagedTeacherResponseDto
        {
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            Items = teachers
        };
    }
}
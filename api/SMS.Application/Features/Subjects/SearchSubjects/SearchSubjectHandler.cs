using SMS.Application.Features.Subjects.GetSubject;
using Microsoft.EntityFrameworkCore;
using SMS.Application.DTOs;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Subjects.SearchSubjects;

public class SearchSubjectHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public SearchSubjectHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<PagedSubjectResponseDto> HandleAsync(
        GetSubjectQuery query,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }

        var subjectsQuery = _dbContext.Subjects
            .Where(s => s.TenantId == tenantId.Value)
            .Where(s => s.IsActive);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            subjectsQuery = subjectsQuery.Where(s =>
                s.Name.Contains(query.Search) ||
                s.Code.Contains(query.Search));
        }

        var totalCount = await subjectsQuery.CountAsync(
            cancellationToken);

        var subjects = await subjectsQuery
            .OrderBy(s => s.Name)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(s => new SubjectListItemDto
            {
                Id = s.Id,
                Name = s.Name,
                Code = s.Code
            })
            .ToListAsync(cancellationToken);

        return new PagedSubjectResponseDto
        {
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            Items = subjects
        };
    }
}

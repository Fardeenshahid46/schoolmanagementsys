using Microsoft.EntityFrameworkCore;
using SMS.Application.DTOs;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;
using SMS.Application.Features.Classes.GetClass;

namespace SMS.Application.Features.Classes.SearchClasses;

public class SearchClassHandler
{
    private readonly AppDbContext _dbContext;
    private readonly ITenantProvider _tenantProvider;

    public SearchClassHandler(
        AppDbContext dbContext,
        ITenantProvider tenantProvider)
    {
        _dbContext = dbContext;
        _tenantProvider = tenantProvider;
    }

    public async Task<PagedClassResponseDto> HandleAsync(
        GetClassQuery query,
        CancellationToken cancellationToken)
    {
        var tenantId = _tenantProvider.TenantId;

        if (!tenantId.HasValue)
        {
            throw new InvalidOperationException(
                "Tenant not resolved.");
        }

        var classesQuery = _dbContext.Classes
            .Where(c => c.TenantId == tenantId.Value)
            .Where(c => c.IsActive);

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            classesQuery = classesQuery.Where(c =>
                c.Name.Contains(query.Search) ||
                c.Section.Contains(query.Search));
        }

        var totalRecords = await classesQuery
            .CountAsync(cancellationToken);

        var classes = await classesQuery
            .OrderBy(c => c.Name)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(c => new ClassListItemDto
            {
                Id = c.Id,
                Name = c.Name,
                Section = c.Section
            })
            .ToListAsync(cancellationToken);

        return new PagedClassResponseDto
        {
            TotalRecords = totalRecords,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            Classes = classes
        };
    }
}
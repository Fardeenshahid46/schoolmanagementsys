using Microsoft.AspNetCore.Http;
using SMS.Domain.Interfaces;

namespace SMS.Infrastructure.TenantResolution;

public class TenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? TenantId =>
        _httpContextAccessor.HttpContext?.Items["TenantId"] as Guid?;
}

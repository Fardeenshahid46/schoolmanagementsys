using SMS.Domain.Interfaces;
using SMS.Infrastructure.TenantResolution;
using SMS.API.Middleware;

namespace SMS.API.Extensions;

public static class TenantServiceExtensions
{
    public static IServiceCollection AddTenantResolution(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ITenantProvider, TenantProvider>();
        return services;
    }

    public static IApplicationBuilder UseTenantResolution(this IApplicationBuilder app)
    {
        app.UseMiddleware<TenantResolutionMiddleware>();
        return app;
    }
}

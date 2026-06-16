using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SMS.Persistence.Context;

namespace SMS.API.Middleware;

public class TenantResolutionMiddleware
{
    private readonly RequestDelegate _next;

    // Paths that bypass tenant resolution
    private static readonly string[] ExcludedPaths =
    [
        "/swagger",
        "/api/auth/login"
    ];

    public TenantResolutionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
    {
        // Skip tenant resolution for excluded paths
        var path = context.Request.Path.Value?.ToLowerInvariant() ?? string.Empty;
        if (ExcludedPaths.Any(excluded => path.StartsWith(excluded, StringComparison.OrdinalIgnoreCase)))
        {
            await _next(context);
            return;
        }

        // Return 400 if X-Tenant-Id header is missing
        if (!context.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantIdHeader)
            || string.IsNullOrWhiteSpace(tenantIdHeader.ToString()))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { error = "X-Tenant-Id header is required." });
            return;
        }

        // Return 400 if header value is not a valid GUID
        if (!Guid.TryParse(tenantIdHeader.ToString(), out var tenantId))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { error = "X-Tenant-Id header format is invalid." });
            return;
        }

        // Return 404 if tenant does not exist in database
        var tenantExists = await dbContext.Tenants.AnyAsync(t => t.Id == tenantId && t.IsActive);
        if (!tenantExists)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { error = "Tenant not found." });
            return;
        }

        // Store TenantId in HttpContext.Items for downstream access
        context.Items["TenantId"] = tenantId;

        await _next(context);
    }
}

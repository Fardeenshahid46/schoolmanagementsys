namespace SMS.Domain.Entities;

public class SchoolClass
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Section { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public Tenant Tenant { get; set; } = null!;
}
namespace SMS.Domain.Entities;

public class Subject
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Code { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    // Navigation Property
    public Tenant Tenant { get; set; } = null!;
}

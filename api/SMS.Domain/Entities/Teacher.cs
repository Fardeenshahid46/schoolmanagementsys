namespace SMS.Domain.Entities;

public class Teacher
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Qualification { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public Tenant Tenant { get; set; } = null!;
}
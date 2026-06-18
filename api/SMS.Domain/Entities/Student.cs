namespace SMS.Domain.Entities;

public class Student
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string AdmissionNumber { get; set; } = string.Empty;

    public string GuardianName { get; set; } = string.Empty;

    public string GuardianPhone { get; set; } = string.Empty;

    public string? Address { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    // Navigation Property
    public Tenant Tenant { get; set; } = null!;
}
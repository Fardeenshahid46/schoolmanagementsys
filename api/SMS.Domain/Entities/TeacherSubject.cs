namespace SMS.Domain.Entities;

public class TeacherSubject
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public Guid TeacherId { get; set; }

    public Guid SubjectId { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public Tenant Tenant { get; set; } = null!;

    public Teacher Teacher { get; set; } = null!;

    public Subject Subject { get; set; } = null!;
}

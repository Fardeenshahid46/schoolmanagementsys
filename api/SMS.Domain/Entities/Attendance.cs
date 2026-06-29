namespace SMS.Domain.Entities;

public class Attendance
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public Guid StudentId { get; set; }

    public Guid ClassId { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime AttendanceDate { get; set; }

    public string? Remarks { get; set; }

    public bool IsActive { get; set; } = true;


    // Navigation Properties

    public Tenant Tenant { get; set; } = null!;

    public Student Student { get; set; } = null!;

    public SchoolClass SchoolClass { get; set; } = null!;
}
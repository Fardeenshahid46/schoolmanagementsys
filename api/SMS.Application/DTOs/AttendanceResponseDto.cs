namespace SMS.Application.DTOs;

public class AttendanceResponseDto
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public Guid StudentId { get; set; }

    public Guid ClassId { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime AttendanceDate { get; set; }

    public string? Remarks { get; set; }

    public bool IsActive { get; set; }
}

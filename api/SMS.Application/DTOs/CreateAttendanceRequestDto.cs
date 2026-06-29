namespace SMS.Application.DTOs;

public class CreateAttendanceRequestDto
{
    public Guid StudentId { get; set; }

    public Guid ClassId { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime AttendanceDate { get; set; }

    public string? Remarks { get; set; }
}

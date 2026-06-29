namespace SMS.Application.Features.Attendance.GetAttendance;

public class AttendanceListItemDto
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public string StudentName { get; set; } = string.Empty;

    public Guid ClassId { get; set; }

    public string ClassName { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime AttendanceDate { get; set; }

    public string? Remarks { get; set; }
}

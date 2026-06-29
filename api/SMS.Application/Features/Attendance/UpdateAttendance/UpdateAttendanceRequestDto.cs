namespace SMS.Application.Features.Attendance.UpdateAttendance;

public class UpdateAttendanceRequestDto
{
    public string Status { get; set; } = string.Empty;

    public DateTime AttendanceDate { get; set; }

    public string? Remarks { get; set; }
}

namespace SMS.Application.Features.Attendance.CreateAttendance;

public record CreateAttendanceCommand(
    Guid StudentId,
    Guid ClassId,
    string Status,
    DateTime AttendanceDate,
    string? Remarks);

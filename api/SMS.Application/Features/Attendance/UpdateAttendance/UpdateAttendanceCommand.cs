namespace SMS.Application.Features.Attendance.UpdateAttendance;

public record UpdateAttendanceCommand(
    Guid Id,
    string Status,
    DateTime AttendanceDate,
    string? Remarks);

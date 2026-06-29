namespace SMS.Application.Features.Attendance.GetAttendance;

public record GetAttendanceQuery(
    Guid? StudentId,
    Guid? ClassId);

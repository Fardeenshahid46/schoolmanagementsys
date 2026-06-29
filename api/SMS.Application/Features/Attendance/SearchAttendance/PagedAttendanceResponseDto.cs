using SMS.Application.Features.Attendance.GetAttendance;

namespace SMS.Application.Features.Attendance.SearchAttendance;

public class PagedAttendanceResponseDto
{
    public int TotalCount { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public List<AttendanceListItemDto> Items { get; set; } = new();
}

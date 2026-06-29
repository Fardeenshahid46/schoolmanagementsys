namespace SMS.Application.Features.Attendance.SearchAttendance;

public class SearchAttendanceQuery
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? Search { get; set; }

    public Guid? StudentId { get; set; }

    public Guid? ClassId { get; set; }

    public DateTime? Date { get; set; }
}

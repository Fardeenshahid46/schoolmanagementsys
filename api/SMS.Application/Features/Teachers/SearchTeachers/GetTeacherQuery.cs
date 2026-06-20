namespace SMS.Application.Features.Teachers.SearchTeachers;

public class GetTeacherQuery
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? Search { get; set; }
}
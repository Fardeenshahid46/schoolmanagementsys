namespace SMS.Application.Features.TeacherSubjects.SearchTeacherSubjects;

public class GetTeacherSubjectQuery
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? Search { get; set; }
}

namespace SMS.Application.Features.Subjects.SearchSubjects;

public class GetSubjectQuery
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? Search { get; set; }
}

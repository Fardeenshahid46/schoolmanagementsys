namespace SMS.Application.Features.Classes.SearchClasses;

public class GetClassQuery
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? Search { get; set; }
}
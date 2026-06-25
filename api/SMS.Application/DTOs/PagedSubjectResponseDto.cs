using SMS.Application.Features.Subjects.GetSubject;

namespace SMS.Application.DTOs;

public class PagedSubjectResponseDto
{
    public int TotalCount { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public List<SubjectListItemDto> Items { get; set; }
        = new();
}

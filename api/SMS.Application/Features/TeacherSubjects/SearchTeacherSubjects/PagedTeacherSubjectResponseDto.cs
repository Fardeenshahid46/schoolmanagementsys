using SMS.Application.Features.TeacherSubjects.GetTeacherSubjects;

namespace SMS.Application.Features.TeacherSubjects.SearchTeacherSubjects;

public class PagedTeacherSubjectResponseDto
{
    public int TotalCount { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public List<TeacherSubjectListItemDto> Items { get; set; } = new();
}

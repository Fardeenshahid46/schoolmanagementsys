using SMS.Application.Features.Teachers.GetTeacher;

namespace SMS.Application.Features.Teachers.SearchTeachers;

public class PagedTeacherResponseDto
{
    public int TotalCount { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public List<TeacherListItemDto> Items { get; set; } = new();
}

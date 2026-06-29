using SMS.Application.Features.StudentClasses.GetStudentClasses;

namespace SMS.Application.Features.StudentClasses.SearchStudentClasses;

public class PagedStudentClassResponseDto
{
    public int TotalCount { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public List<StudentClassListItemDto> Items { get; set; } = new();
}

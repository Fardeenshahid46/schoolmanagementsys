using SMS.Application.Features.Students.GetStudent;

namespace SMS.Application.Features.Students.SearchStudents;

public class PagedStudentResponseDto
{
    public int TotalCount { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public List<StudentListItemDto> Items { get; set; } = new();
}

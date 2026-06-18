using SMS.Application.Features.Students.GetStudent;
namespace SMS.Application.DTOs;

public class PagedStudentResponseDto
{
    public int TotalRecords { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public List<StudentListItemDto> Students { get; set; }
        = new();
}
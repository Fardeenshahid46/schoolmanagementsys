using SMS.Application.Features.Teachers.GetTeacher;

namespace SMS.Application.DTOs;

public class PagedTeacherResponseDto
{
    public int TotalRecords { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public List<TeacherListItemDto> Teachers { get; set; }
        = new();
}
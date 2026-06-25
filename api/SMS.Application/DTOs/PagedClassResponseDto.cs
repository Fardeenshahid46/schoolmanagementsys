using SMS.Application.Features.Classes.GetClass;

namespace SMS.Application.DTOs;

public class PagedClassResponseDto
{
    public int TotalRecords { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public List<ClassListItemDto> Classes { get; set; }
        = new();
}
namespace SMS.Application.DTOs;

public class StudentClassResponseDto
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public Guid ClassId { get; set; }

    public bool IsActive { get; set; }
}

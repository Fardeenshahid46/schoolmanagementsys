namespace SMS.Application.DTOs;

public class TeacherSubjectResponseDto
{
    public Guid Id { get; set; }

    public Guid TeacherId { get; set; }

    public Guid SubjectId { get; set; }

    public bool IsActive { get; set; }
}

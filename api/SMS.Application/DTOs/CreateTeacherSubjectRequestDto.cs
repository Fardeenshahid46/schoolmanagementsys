namespace SMS.Application.DTOs;

public class CreateTeacherSubjectRequestDto
{
    public Guid TeacherId { get; set; }

    public Guid SubjectId { get; set; }
}

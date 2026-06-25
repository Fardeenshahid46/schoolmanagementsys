namespace SMS.Application.Features.TeacherSubjects.GetTeacherSubjects;

public class TeacherSubjectListItemDto
{
    public Guid Id { get; set; }

    public Guid TeacherId { get; set; }

    public string TeacherName { get; set; } = string.Empty;

    public Guid SubjectId { get; set; }

    public string SubjectName { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}

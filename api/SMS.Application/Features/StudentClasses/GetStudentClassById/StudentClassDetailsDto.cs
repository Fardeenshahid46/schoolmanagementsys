namespace SMS.Application.Features.StudentClasses.GetStudentClassById;

public class StudentClassDetailsDto
{
    public Guid Id { get; set; }

    public Guid StudentId { get; set; }

    public string StudentName { get; set; } = string.Empty;

    public Guid ClassId { get; set; }

    public string ClassName { get; set; } = string.Empty;

    public string Section { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}

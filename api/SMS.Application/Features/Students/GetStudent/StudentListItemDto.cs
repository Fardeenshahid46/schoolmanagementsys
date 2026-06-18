namespace SMS.Application.Features.Students.GetStudent;

public class StudentListItemDto
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string AdmissionNumber { get; set; } = string.Empty;

    public string GuardianName { get; set; } = string.Empty;

    public string GuardianPhone { get; set; } = string.Empty;
}
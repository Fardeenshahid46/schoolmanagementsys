namespace SMS.Application.DTOs;

public class CreateStudentRequestDto
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string AdmissionNumber { get; set; } = string.Empty;

    public string GuardianName { get; set; } = string.Empty;

    public string GuardianPhone { get; set; } = string.Empty;

    public string? Address { get; set; }
}
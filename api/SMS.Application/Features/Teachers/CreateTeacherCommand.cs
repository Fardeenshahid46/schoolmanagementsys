namespace SMS.Application.Features.Teachers.CreateTeacher;

public record CreateTeacherCommand(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string Qualification,
    string Address);
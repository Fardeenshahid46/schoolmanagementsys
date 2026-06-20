namespace SMS.Application.DTOs;

public class UpdateTeacherRequestDto
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Qualification { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;
}
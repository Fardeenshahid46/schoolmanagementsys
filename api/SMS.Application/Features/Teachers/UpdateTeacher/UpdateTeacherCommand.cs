namespace SMS.Application.Features.Teachers.UpdateTeacher;

public class UpdateTeacherCommand
{
    public Guid Id { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public string Email { get; }

    public string PhoneNumber { get; }

    public string Qualification { get; }

    public string Address { get; }

    public UpdateTeacherCommand(
        Guid id,
        string firstName,
        string lastName,
        string email,
        string phoneNumber,
        string qualification,
        string address)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PhoneNumber = phoneNumber;
        Qualification = qualification;
        Address = address;
    }
}
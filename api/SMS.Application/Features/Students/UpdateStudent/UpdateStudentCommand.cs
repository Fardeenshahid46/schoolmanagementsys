namespace SMS.Application.Features.Students.UpdateStudent;


public class UpdateStudentCommand
{
    public Guid Id { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public string Gender { get; }

    public DateTime DateOfBirth { get; }

    public string GuardianName { get; }

    public string GuardianPhone { get; }

    public string? Address { get; }


    public UpdateStudentCommand(
        Guid id,
        string firstName,
        string lastName,
        string gender,
        DateTime dateOfBirth,
        string guardianName,
        string guardianPhone,
        string? address)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        GuardianName = guardianName;
        GuardianPhone = guardianPhone;
        Address = address;
    }
}
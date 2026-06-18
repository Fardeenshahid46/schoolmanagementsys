namespace SMS.Application.Features.Students.CreateStudent;

public class CreateStudentCommand
{
    public string FirstName { get; }
    public string LastName { get; }
    public string Gender { get; }
    public DateTime DateOfBirth { get; }
    public string AdmissionNumber { get; }
    public string GuardianName { get; }
    public string GuardianPhone { get; }
    public string? Address { get; }

    public CreateStudentCommand(
        string firstName,
        string lastName,
        string gender,
        DateTime dateOfBirth,
        string admissionNumber,
        string guardianName,
        string guardianPhone,
        string? address)
    {
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        AdmissionNumber = admissionNumber;
        GuardianName = guardianName;
        GuardianPhone = guardianPhone;
        Address = address;
    }
}
namespace SMS.Application.Features.Students.DeleteStudent;

public class DeleteStudentCommand
{
    public Guid Id { get; }

    public DeleteStudentCommand(Guid id)
    {
        Id = id;
    }
}   
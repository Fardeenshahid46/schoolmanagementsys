namespace SMS.Application.Features.Teachers.DeleteTeacher;

public class DeleteTeacherCommand
{
    public Guid Id { get; }

    public DeleteTeacherCommand(Guid id)
    {
        Id = id;
    }
}
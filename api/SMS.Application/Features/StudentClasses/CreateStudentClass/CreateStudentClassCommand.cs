namespace SMS.Application.Features.StudentClasses.CreateStudentClass;

public record CreateStudentClassCommand(
    Guid StudentId,
    Guid ClassId);

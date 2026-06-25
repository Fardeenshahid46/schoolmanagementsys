namespace SMS.Application.Features.Subjects.UpdateSubject;

public record UpdateSubjectCommand(
    Guid Id,
    string Name,
    string Code);

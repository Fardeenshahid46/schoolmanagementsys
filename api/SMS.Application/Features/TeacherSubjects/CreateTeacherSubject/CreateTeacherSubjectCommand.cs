namespace SMS.Application.Features.TeacherSubjects.CreateTeacherSubject;

public record CreateTeacherSubjectCommand(
    Guid TeacherId,
    Guid SubjectId);

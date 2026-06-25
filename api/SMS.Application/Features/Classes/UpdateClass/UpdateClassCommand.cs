namespace SMS.Application.Features.Classes.UpdateClass;

public record UpdateClassCommand(
    Guid Id,
    string Name,
    string Section);
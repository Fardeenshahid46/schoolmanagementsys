namespace SMS.Application.Features.Classes.GetClass;

public class ClassListItemDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Section { get; set; } = string.Empty;
}
namespace SMS.Domain.Entities;

public class Tenant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Subdomain { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

public ICollection<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();

public ICollection<Student> Students { get; set; } = new List<Student>();
}

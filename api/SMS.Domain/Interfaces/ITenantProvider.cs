namespace SMS.Domain.Interfaces;

public interface ITenantProvider
{
    Guid? TenantId { get; }
}

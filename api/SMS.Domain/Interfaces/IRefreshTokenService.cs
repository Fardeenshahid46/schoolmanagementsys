using SMS.Domain.Entities;

namespace SMS.Domain.Interfaces;

public interface IRefreshTokenService
{
    RefreshToken GenerateRefreshToken(Guid userId);
}

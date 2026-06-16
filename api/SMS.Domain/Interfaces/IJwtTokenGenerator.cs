using SMS.Domain.Entities;

namespace SMS.Domain.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateAccessToken(ApplicationUser user);
}

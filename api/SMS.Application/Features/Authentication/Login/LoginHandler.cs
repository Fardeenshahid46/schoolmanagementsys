using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SMS.Application.DTOs;
using SMS.Domain.Interfaces;
using SMS.Persistence.Context;

namespace SMS.Application.Features.Authentication.Login;

public class LoginHandler
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IConfiguration _configuration;

    public LoginHandler(
        AppDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        IRefreshTokenService refreshTokenService,
        IConfiguration configuration)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenService = refreshTokenService;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto?> HandleAsync(LoginCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _context.ApplicationUsers
            .FirstOrDefaultAsync(u => u.Email == command.Email, cancellationToken);

        if (user == null)
        {
            return null;
        }

        var isPasswordValid = _passwordHasher.VerifyPassword(command.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            return null;
        }

        var accessToken = _jwtTokenGenerator.GenerateAccessToken(user);
        var refreshTokenEntity = _refreshTokenService.GenerateRefreshToken(user.Id);

        // Add refresh token to persistence and save
        _context.RefreshTokens.Add(refreshTokenEntity);
        await _context.SaveChangesAsync(cancellationToken);

        // Retrieve token expiration details to populate LoginResponseDto
        var expirationMinutesSetting = _configuration["AuthenticationSettings:AccessTokenExpirationMinutes"];
        var expirationMinutes = int.TryParse(expirationMinutesSetting, out var minutes) ? minutes : 60;
        var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenEntity.Token,
            Expiration = expiration
        };
    }
}

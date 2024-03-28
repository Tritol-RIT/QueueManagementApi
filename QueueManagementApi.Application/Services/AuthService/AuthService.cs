using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QueueManagementApi.Application.Services.EncryptionService;
using QueueManagementApi.Application.Services.TokenService;
using QueueManagementApi.Core;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Core.Extensions;
using QueueManagementApi.Core.Interfaces;
using System.Security.Claims;

namespace QueueManagementApi.Application.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepository;
    private readonly ITokenService _tokenService; // Handles token generation and validation
    private readonly IEncryptionService _encryptionService;

    public AuthService(IRepository<User> userRepository, ITokenService tokenService, IEncryptionService encryptionService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _encryptionService = encryptionService;
    }

    public async Task<(string accessToken, string refreshToken)> LoginAsync(string email, string password)
    {
        var user = await _userRepository.FindByCondition(x => x.Email == email && x.Active && x.PasswordHash != null).FirstOrDefaultAsync();
        if (user == null || !_encryptionService.VerifyPassword(password, user.PasswordHash!))
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        // Generate OAuth token
        var accessToken = _tokenService.GenerateToken(user);

        // Generate a refresh token
        var refreshToken = _tokenService.GenerateRefreshToken(user);

        return (accessToken, refreshToken);
    }

    public async Task<string> RefreshTokenAsync(string refreshToken)
    {
        // Validate the refresh token (e.g., expiration, signature, and if it's been revoked)
        var principal = _tokenService.ValidateRefreshToken(refreshToken);
        if (principal == null)
        {
            throw new SecurityTokenException("Invalid refresh token");
        }

        // Extract user information from the refresh token's claims
        var userId = principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value.AsInt();

        // Retrieve the user based on the userId claim
        var user = await _userRepository.FindById(userId);
        if (user == null)
        {
            throw new QueueApiException("User does not exist");
        }

        // Generate and return a new access token
        return _tokenService.GenerateToken(user);
    }
}
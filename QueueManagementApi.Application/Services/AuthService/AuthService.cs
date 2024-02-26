using Microsoft.EntityFrameworkCore;
using QueueManagementApi.Application.Services.EncryptionService;
using QueueManagementApi.Application.Services.TokenService;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Core.Interfaces;

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

    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await _userRepository.FindByCondition(x => x.Email == email).FirstOrDefaultAsync();
        if (user == null || !_encryptionService.VerifyPassword(password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        // Generate OAuth token
        return _tokenService.GenerateToken(user);
    }
}
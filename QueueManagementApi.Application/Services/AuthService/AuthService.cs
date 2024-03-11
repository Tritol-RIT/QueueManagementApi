using Microsoft.EntityFrameworkCore;
using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Application.Services.EncryptionService;
using QueueManagementApi.Application.Services.TokenService;
using QueueManagementApi.Core;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Core.Interfaces;

namespace QueueManagementApi.Application.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepository;
    private readonly ITokenService _tokenService; // Handles token generation and validation
    private readonly IEncryptionService _encryptionService;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IRepository<User> userRepository, ITokenService tokenService, IEncryptionService encryptionService, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _encryptionService = encryptionService;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await _userRepository.FindByCondition(x => x.Email == email && x.Active).FirstOrDefaultAsync();
        if (user == null || !_encryptionService.VerifyPassword(password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        // Generate OAuth token
        return _tokenService.GenerateToken(user);
    }

    public async Task<User> CreateUser(CreateUserDto createUserDto)
    {
        var userExists = await _userRepository.FindByCondition(x => x.Email == createUserDto.Email).AnyAsync();
        if (userExists)
            throw new QueueApiException("User with this email already exists");

        if (createUserDto.UserRole == UserRole.Staff)
        {
            var exhibitExists = await 
                _unitOfWork.Repository<Exhibit>()
                    .FindByCondition(x => x.Id == createUserDto.ExhibitId)
                    .AnyAsync();
            if (!exhibitExists)
                throw new QueueApiException("Exhibit for staff does not exist");
        }

        var userPassword = _encryptionService.GenerateRandomPassword();

        // send email here

        var newUser = new User
        {
            Email = createUserDto.Email,
            FirstName = createUserDto.Name,
            LastName = createUserDto.Surname,
            ExhibitId = createUserDto.ExhibitId,
            PasswordHash = _encryptionService.HashPassword(userPassword),
            Role = createUserDto.UserRole
        };

        await _userRepository.AddAsync(newUser);
        await _unitOfWork.CompleteAsync();

        return newUser;
    }
}
using Microsoft.EntityFrameworkCore;
using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Application.Services.EmailService;
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
    private readonly IEmailService _emailService;

    public AuthService(IRepository<User> userRepository, ITokenService tokenService, IEncryptionService encryptionService, IUnitOfWork unitOfWork, IEmailService emailService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _encryptionService = encryptionService;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
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

        // send email here
        await _emailService.SendEmailUserAsync(createUserDto.Email, "New User", newUser, userPassword);

        return newUser;
    }

    public async Task<User?> UpdateUserAsync(int id, UserUpdateDto userUpdateDto)
    {
        var user = await _userRepository.FindById(id);
        if (user is null)
            throw new QueueApiException("User was not found");

        user.Active = userUpdateDto.Active;
        user.ExhibitId = userUpdateDto.ExhibitId;
        user.FirstName = userUpdateDto.FirstName;
        user.LastName = userUpdateDto.LastName;

        _userRepository.Update(user);
        await _unitOfWork.CompleteAsync();

        return user;
    }
}
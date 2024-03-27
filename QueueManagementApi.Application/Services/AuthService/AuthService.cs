using Microsoft.EntityFrameworkCore;
using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Application.Services.EmailService;
using QueueManagementApi.Application.Services.EncryptionService;
using QueueManagementApi.Application.Services.SetPasswordTokenService;
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
    private readonly ISetPasswordTokenService _setPasswordTokenService;

    public AuthService(ITokenService tokenService, IEncryptionService encryptionService, IUnitOfWork unitOfWork, IEmailService emailService, ISetPasswordTokenService setPasswordTokenService)
    {
        _userRepository = unitOfWork.Repository<User>();
        _tokenService = tokenService;
        _unitOfWork = unitOfWork;
        _encryptionService = encryptionService;
        _emailService = emailService;
        _setPasswordTokenService = setPasswordTokenService;
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

        var newUser = new User
        {
            Email = createUserDto.Email,
            FirstName = createUserDto.Name,
            LastName = createUserDto.Surname,
            ExhibitId = createUserDto.ExhibitId,
            Role = createUserDto.UserRole,
            Active = false // turn to true after user sets a password
        };

        await _userRepository.AddAsync(newUser);

        var setPasswordToken = new SetPasswordToken()
            {
                User = newUser,
                ExpirationDate = DateTime.UtcNow.AddMinutes(15),
                Token = Guid.NewGuid().ToString(),
                Active = true
            };

        await _unitOfWork.Repository<SetPasswordToken>().AddAsync(setPasswordToken);

        await _unitOfWork.CompleteAsync();
        // send email here
        await _emailService.SendEmailUserAsync(createUserDto.Email, "New User", newUser, setPasswordToken.Token);

        return newUser;
    }

    public async Task<User?> UpdateUserAsync(int id, UserUpdateDto userUpdateDto)
    {
        var user = await _userRepository.FindById(id);
        if (user is null)
            throw new QueueApiException("User was not found");

        user.Active = userUpdateDto.Active;
        if (user.ExhibitId != userUpdateDto.ExhibitId)
        {
            var exhibitExists = await
                _unitOfWork.Repository<Exhibit>()
                    .FindByCondition(x => x.Id == userUpdateDto.ExhibitId)
                    .AnyAsync();
            if (!exhibitExists)
                throw new QueueApiException("Exhibit for staff does not exist");
        }

        user.ExhibitId = userUpdateDto.ExhibitId;
        user.FirstName = userUpdateDto.FirstName;
        user.LastName = userUpdateDto.LastName;

        _userRepository.Update(user);
        await _unitOfWork.CompleteAsync();

        return user;
    }

    public async Task SetPassword(string token, string newPassword)
    {
        var tokenIsValid = await _setPasswordTokenService.ValidateAsync(token);

        if (!tokenIsValid) 
            throw new QueueApiException("This link is expired, request new password again!");

        var setPasswordToken = await _unitOfWork.Repository<SetPasswordToken>().FindByCondition(x => x.Token == token).Include(x => x.User).FirstAsync();

        var userToUpdate = setPasswordToken.User;

        userToUpdate.PasswordHash = _encryptionService.HashPassword(newPassword);
        userToUpdate.Active = true;
        setPasswordToken.Active = false;

        _userRepository.Update(userToUpdate);
        _unitOfWork.Repository<SetPasswordToken>().Update(setPasswordToken);

        await _unitOfWork.CompleteAsync();
    }
}
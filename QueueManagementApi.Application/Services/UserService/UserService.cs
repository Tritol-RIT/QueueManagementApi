using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QueueManagementApi.Application.Services.EmailService;
using QueueManagementApi.Application.Services.EncryptionService;
using QueueManagementApi.Application.Services.SetPasswordTokenService;
using QueueManagementApi.Core.Interfaces;
using QueueManagementApi.Core.Enums;
using QueueManagementApi.Core.Extensions;
using QueueManagementApi.Core.Pagination;

namespace QueueManagementApi.Application.Services.UserService;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISetPasswordTokenService _setPasswordTokenService;
    private readonly IEncryptionService _encryptionService;
    private readonly IEmailService _emailService;

    public UserService(IUnitOfWork unitOfWork, ISetPasswordTokenService setPasswordTokenService, IEncryptionService encryptionService, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _setPasswordTokenService = setPasswordTokenService;
        _encryptionService = encryptionService;
        _emailService = emailService;

        _userRepository = unitOfWork.Repository<User>();
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

        var setPasswordToken = await _unitOfWork
            .Repository<SetPasswordToken>()
            .FindByCondition(x => x.Token == token)
            .Include(x => x.User)
            .FirstAsync(); // it cannot be null here because it would have failed in the `SetPasswordTokenService.ValidateAsync` method

        var userToUpdate = setPasswordToken.User;

        userToUpdate.PasswordHash = _encryptionService.HashPassword(newPassword);
        userToUpdate.Active = true; // activate user
        setPasswordToken.Active = false; // deactivate token so it is not used twice

        _userRepository.Update(userToUpdate);
        _unitOfWork.Repository<SetPasswordToken>().Update(setPasswordToken);

        await _unitOfWork.CompleteAsync();
    }

    public async Task RequestResetPassword(string email)
    {
        // Find the user by email
        var user = await _userRepository.FindByCondition(user => user.Email == email).FirstOrDefaultAsync();
        if (user == null)
            throw new QueueApiException("User not found!");

        // Generate a new token for password reset
        var resetPasswordToken = new SetPasswordToken
        {
            User = user,
            ExpirationDate = DateTime.UtcNow.AddMinutes(15),
            Token = Guid.NewGuid().ToString(),
            Active = true
        };

        user.Active = false; // When reset password is requested, that user is deactivated until they reset their password

        _userRepository.Update(user);
        await _unitOfWork.Repository<SetPasswordToken>().AddAsync(resetPasswordToken);

        await _unitOfWork.CompleteAsync();

        // Send email here
        await _emailService.SendUserResetPasswordEmailAsync(email, "Request for Resetting Password", user, resetPasswordToken.Token);
    }

    public async Task<PagedList<User>> GetUsers(int page, int pageSize, string? search)
    {
        var res = _userRepository
            .GetAll();

        if (!search.IsEmpty())
        {
            res = res.Where(
                x => x.FirstName.Contains(search) || x.LastName.Contains(search) || x.Email.Contains(search));
        }

        res = res.OrderByDescending(x => x.CreatedOn);

        return res.ToPagedList(page, pageSize);
    }
}
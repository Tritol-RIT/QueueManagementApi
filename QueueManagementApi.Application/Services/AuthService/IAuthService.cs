using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Core.Entities;

namespace QueueManagementApi.Application.Services.AuthService;

public interface IAuthService
{
    Task<string> LoginAsync(string email, string password);

    Task<User> CreateUser(CreateUserDto  createUserDto);
    Task<User?> UpdateUserAsync(int id, UserUpdateDto userUpdateDto);
    Task SetPassword(string token, string newPassword);
}
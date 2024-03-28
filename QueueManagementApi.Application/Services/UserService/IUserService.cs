using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Core.Entities;

namespace QueueManagementApi.Application.Services.UserService;

public interface IUserService
{
    Task<User> CreateUser(CreateUserDto createUserDto);
    Task<User?> UpdateUserAsync(int id, UserUpdateDto userUpdateDto);
    Task SetPassword(string token, string newPassword);
}
using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Core.Pagination;

namespace QueueManagementApi.Application.Services.UserService;

public interface IUserService
{
    Task<User> CreateUser(CreateUserDto createUserDto);
    Task<User?> UpdateUserAsync(int id, UserUpdateDto userUpdateDto);
    Task SetPassword(string token, string newPassword);
    Task RequestResetPassword(string email);
    Task<PagedList<User>> GetUsers(int page, int pageSize, string? search);
}
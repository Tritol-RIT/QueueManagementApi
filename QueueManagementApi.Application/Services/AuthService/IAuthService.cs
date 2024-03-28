using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Core.Entities;

namespace QueueManagementApi.Application.Services.AuthService;

public interface IAuthService
{
    Task<(string accessToken, string refreshToken)> LoginAsync(string email, string password);
    Task<string> RefreshTokenAsync(string refreshToken);
}
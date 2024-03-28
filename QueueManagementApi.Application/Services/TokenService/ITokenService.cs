using QueueManagementApi.Core.Entities;
using System.Security.Claims;

namespace QueueManagementApi.Application.Services.TokenService;
public interface ITokenService
{
    string GenerateToken(User user);
    string GenerateRefreshToken(User user);
    ClaimsPrincipal ValidateRefreshToken(string refreshToken);
}
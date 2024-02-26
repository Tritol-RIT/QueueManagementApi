using QueueManagementApi.Core.Entities;

namespace QueueManagementApi.Application.Services.TokenService;
public interface ITokenService
{
    string GenerateToken(User user);
}
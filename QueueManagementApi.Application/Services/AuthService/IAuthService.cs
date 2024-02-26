namespace QueueManagementApi.Application.Services.AuthService;

public interface IAuthService
{
    Task<string> LoginAsync(string email, string password);
}
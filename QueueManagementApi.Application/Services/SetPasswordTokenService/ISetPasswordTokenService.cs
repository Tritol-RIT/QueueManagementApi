namespace QueueManagementApi.Application.Services.SetPasswordTokenService;

public interface ISetPasswordTokenService
{
    Task<bool> ValidateAsync(string token);
}
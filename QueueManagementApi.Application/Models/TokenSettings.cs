namespace QueueManagementApi.Application.Models;

public class TokenSettings
{
    public string Secret { get; set; }
    public int ExpirationHours { get; set; }
    public int RefreshTokenExpirationHours { get; set; }
}
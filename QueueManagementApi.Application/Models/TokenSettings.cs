namespace QueueManagementApi.Application.Models;

public class TokenSettings
{
    public string Secret { get; set; }
    public int ExpirationHours { get; set; }
}
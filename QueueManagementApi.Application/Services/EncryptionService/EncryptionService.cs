namespace QueueManagementApi.Application.Services.EncryptionService;

using BCrypt.Net;

public class EncryptionService : IEncryptionService
{
    public string HashPassword(string password)
    {
        return BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Verify(password, hashedPassword);
    }
}
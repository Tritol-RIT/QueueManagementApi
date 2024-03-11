namespace QueueManagementApi.Application.Services.EncryptionService;

using BCrypt.Net;
using System.Security.Cryptography;

public class EncryptionService : IEncryptionService
{
    private const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+-=[]{}|;:',.<>?";
    public string HashPassword(string password)
    {
        return BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Verify(password, hashedPassword);
    }

    public string GenerateRandomPassword() => GenerateRandomPassword(25);

    public string GenerateRandomPassword(int length)
    {
        var passwordChars = new char[length];
        var randomBytes = new byte[length];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes); // Fill the array with random bytes
        for (int i = 0; i < length; i++)
        {
            int charIndex = randomBytes[i] % ValidChars.Length; // Use the byte value to select a character
            passwordChars[i] = ValidChars[charIndex];
        }

        return new string(passwordChars);
    }
}
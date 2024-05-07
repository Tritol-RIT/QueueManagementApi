using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using QueueManagementApi.Core.Entities;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using QueueManagementApi.Application.Models;

namespace QueueManagementApi.Application.Services.TokenService;

public class TokenService : ITokenService
{
    private readonly TokenSettings _tokenSettings;

    public TokenService(IOptions<TokenSettings> tokenSettings)
    {
        _tokenSettings = tokenSettings.Value;
    }

    // Generates an access token for a user
    public string GenerateToken(User user)
        => GenerateJwtToken(user, "access", _tokenSettings.ExpirationHours);


    // Generates a refresh token for a user
    public string GenerateRefreshToken(User user) =>
        GenerateJwtToken(user, "refresh", _tokenSettings.RefreshTokenExpirationHours);


    // Private method to generate a JWT token with specified type and expiration
    private string GenerateJwtToken(User user, string tokenType, int expirationHours)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_tokenSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("tokenType", tokenType), // Clearly indicate the token's purpose
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
            }),
            Expires = tokenType == "refresh" ? DateTime.UtcNow.AddHours(expirationHours) : DateTime.UtcNow.AddSeconds(expirationHours), // debug remove AddSeconds
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        if (user.ExhibitId is not null && user.ExhibitId != 0) 
            tokenDescriptor.Claims.Add("exhibitId", user.ExhibitId);

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal ValidateRefreshToken(string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_tokenSettings.Secret);

        var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true, // Ensure the token is not expired
            ClockSkew = TimeSpan.FromMinutes(5) // The tokens will be valid for up to 5 minutes after the exp time
        }, out SecurityToken validatedToken);

        // Additionally, ensure the token is indeed a refresh token
        var tokenType = principal.Claims.FirstOrDefault(c => c.Type == "tokenType")?.Value;
        if (tokenType != "refresh")
        {
            throw new SecurityTokenException("Invalid token type");
        }

        return principal;
    }
}

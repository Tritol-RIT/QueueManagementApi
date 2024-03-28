using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using QueueManagementApi.Application.Models;
using QueueManagementApi.Application.Services.AuthService;
using QueueManagementApi.Core.Extensions;

namespace QueueManagementApi.Controllers;

public class LoginController : ApiController
{
    private readonly IAuthService _authService;

    public LoginController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest("Missing login details.");
        }

        try
        {
            var (token, refreshToken) = await _authService.LoginAsync(request.Email, request.Password);

            return Ok(new
            {
                Token = token,
                RefreshToken = refreshToken
            });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid credentials.");
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        if (request.RefreshToken.IsEmpty())
            return BadRequest("No Refresh Token Sent");
        
        try
        {
            var newAccessToken = await _authService.RefreshTokenAsync(request.RefreshToken);

            return Ok(new
            {
                AccessToken = newAccessToken
            });
        }
        catch (SecurityTokenException ste)
        {
            // Log the specific security token exception message
            return Unauthorized(ste.Message);
        }
    }
}

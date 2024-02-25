using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using QueueManagementApi.Application.Services.AuthService;

namespace QueueManagementApi.Controllers;

public class LoginController : ApiController
{
    private readonly IAuthService _authService;

    public LoginController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
        {
            return BadRequest("Missing login details.");
        }

        try
        {
            var token = await _authService.LoginAsync(request.Email, request.Password);

            return Ok(new { Token = token });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid credentials.");
        }
    }
}

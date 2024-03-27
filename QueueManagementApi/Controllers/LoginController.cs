using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Application.Services.AuthService;
using QueueManagementApi.Application.Services.SetPasswordTokenService;

namespace QueueManagementApi.Controllers;

public class LoginController : ApiController
{
    private readonly IAuthService _authService;
    private readonly ISetPasswordTokenService _setPasswordTokenService;

    public LoginController(IAuthService authService, ISetPasswordTokenService setPasswordTokenService)
    {
        _authService = authService;
        _setPasswordTokenService = setPasswordTokenService;
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
            var token = await _authService.LoginAsync(request.Email, request.Password);

            return Ok(new { Token = token });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid credentials.");
        }
    }

    [HttpPost("create")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto newUser)
    {
        var newUserCreated = await _authService.CreateUser(newUser);
        return Ok(newUserCreated);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Application.Services.UserService;
using QueueManagementApi.Core.Entities;

namespace QueueManagementApi.Controllers;

public class UserController : ApiController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsers(int page, int pageSize, string? search)
    {
        var users = await _userService.GetUsers(page, pageSize, search);
        return Ok(users);
    }

    [HttpPost("create")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto newUser)
    {
        var newUserCreated = await _userService.CreateUser(newUser);
        return Ok(newUserCreated);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto userUpdateDTO)
    {
        var result = await _userService.UpdateUserAsync(id, userUpdateDTO);

        return Ok(result);
    }



    [HttpPut("set-password/{token}")]
    public async Task<IActionResult> SetPassword(string token, [FromBody] string newPassword) // This function should be called for resetting password as well
    {
        if (!IsPasswordValid(newPassword))
            return BadRequest("Password not valid");

        await _userService.SetPassword(token, newPassword); // Removed reset-password function completely, since set-password had the same function

        return Ok("New Password Set Successfully");
    }

    [HttpPost("request-reset-password")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RequestResetPassword(string email)
    {
        await _userService.RequestResetPassword(email);
        return Ok("Request for Resetting Password Sent Successfully");
    }

    private bool IsPasswordValid(string password)
    {
        if (password.Length < 8)
            return false;

        if (!password.Any(char.IsDigit))
            return false;

        if (!password.Any(c => !char.IsLetterOrDigit(c)))
            return false;

        return true;
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Application.Services.UserService;

namespace QueueManagementApi.Controllers;

public class UserController : ApiController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto userUpdateDTO)
    {
        var result = await _userService.UpdateUserAsync(id, userUpdateDTO);

        return Ok(result);
    }

    [HttpPut("set-password/{token}")]
    public async Task<IActionResult> SetPassword(string token, string newPassword)
    {
        // Donat shto validime ne password, ma shum se 8 karaktera, shkronja, numra,etj. ti e din
        // if (not valid passwordi)
        //     return BadRequest("Password not valid");
        await _userService.SetPassword(token, newPassword);

        return Ok("New Password Set Successfully");
    }

    [HttpPost("create")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto newUser)
    {
        var newUserCreated = await _userService.CreateUser(newUser);
        return Ok(newUserCreated);
    }
}
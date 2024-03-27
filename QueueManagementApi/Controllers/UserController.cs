using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Application.Services.AuthService;

namespace QueueManagementApi.Controllers;

public class UserController : ApiController
{
    private readonly IAuthService _authService;

    public UserController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto userUpdateDTO)
    {
        var result = await _authService.UpdateUserAsync(id, userUpdateDTO);

        return Ok(result);
    }

    [HttpPut("set-password/{token}")]
    public async Task<IActionResult> SetPassword(string token, string newPassword)
    {
        // Donat shto validime ne password, ma shum se 8 karaktera, shkronja, numra,etj. ti e din
        // if (not valid passwordi)
        //     return BadRequest("Password not valid");
        await _authService.SetPassword(token, newPassword);

        return Ok("New Password Set Successfully");
    }
}
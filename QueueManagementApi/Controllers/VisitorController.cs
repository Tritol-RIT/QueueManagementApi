using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Application.Services.VisitorService;
using QueueManagementApi.Core.Entities;

namespace QueueManagementApi.Controllers;

public class VisitorController : ApiController
{
    private readonly IVisitorService _visitorService;

    public VisitorController(IVisitorService visitorService)
    {
        _visitorService = visitorService;
    }

    [Authorize(Roles = "Admin,Staff,Committee")]
    [HttpGet("GetAllVisitors")]
    public async Task<IActionResult> GetAction(int page, int pageSize, string? search)
    {
        var result = await _visitorService.VisitGetAll(page, pageSize, search);

        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterVisitor(RegisterVisitorDto registerVisitorDto)
    {
        Visit result = await _visitorService.Register(registerVisitorDto);

        return Ok(result);
    }
}
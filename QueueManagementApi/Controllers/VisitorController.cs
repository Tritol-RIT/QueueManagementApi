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
    [HttpGet("GetAllVisitiors")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAction()
    {
        List<AllVisitDto> result = await _visitorService.VisitGetAll();

        return Ok(result);
    }
    [HttpPost("register")]
    public async Task<IActionResult> RegisterVisitor(RegisterVisitorDto registerVisitorDto)
    {
        Visit result = await _visitorService.Register(registerVisitorDto);

        return Ok(result);
    }
}
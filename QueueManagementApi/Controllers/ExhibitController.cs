using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using QueueManagementApi.Application.Services;
using QueueManagementApi.Core.Entities;

namespace QueueManagementApi.Controllers;

[Route("exhibit")]
public class ExhibitController : ApiController
{
    private readonly IExhibitService _exhibitService;

    public ExhibitController(IExhibitService exhibitService)
    {
        _exhibitService = exhibitService;
    }

    [HttpGet]
    public ActionResult<List<Exhibit>> GetAllExhibits()
    {
        return Ok(_exhibitService.GetAllExhibits());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Exhibit>> GetExhibitById(int id)
    {
        var exhibit = await _exhibitService.GetExhibitById(id);
        if (exhibit == null) return NotFound();
        
        return Ok(exhibit);
    }
}
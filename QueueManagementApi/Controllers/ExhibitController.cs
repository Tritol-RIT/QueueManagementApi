using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using QueueManagementApi.Application.Dtos;
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

    [HttpPost("createSingleExhibit")]
    public async Task<ActionResult<Exhibit>> CreateExhibit(CreateExhibitDto exhibit)
    {
        var exhibitEntity = new Exhibit
        {
            Title = exhibit.Title,
            Description = exhibit.Description,
            MaxCapacity = exhibit.MaxCapacity,
            InitialDuration = exhibit.InitialDuration,
            InsuranceFormRequired = exhibit.InsuranceFormRequired,
            AgeRequired = exhibit.AgeRequired,
            InsuranceFormFileUrl = exhibit.InsuranceFormFileUrl,
        };
        await _exhibitService.AddSingleExhibit(exhibitEntity);
        return CreatedAtAction(nameof(GetExhibitById), new { id = exhibitEntity.Id }, exhibitEntity);
    }
}
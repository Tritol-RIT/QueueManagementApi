using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Application.Services;
using QueueManagementApi.Core.Entities;
using CsvHelper;
using System.Globalization;

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


    [HttpPost("createMultipleExhibits")]
    public async Task<ActionResult> CreateMultipleExhibits([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }
        if (file.ContentType != "text/csv")
        {
            return BadRequest("File is not in the correct format.");
        }

        List<Exhibit> exhibitsList = new List<Exhibit>();
        var config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
        {
            MissingFieldFound = null,
            HeaderValidated = null,
            BadDataFound = null
        };

        using (var reader = new StreamReader(file.OpenReadStream()))
        using (var csv = new CsvReader(reader, config))
        {
            exhibitsList = csv.GetRecords<Exhibit>().ToList();
        }
        
        await _exhibitService.AddMultipleExhibits(exhibitsList);
        return Ok("File is in the correct format and was successfully parsed.");
    }

    [HttpPut("updateSingleExhibit")]
    public async Task<ActionResult<Exhibit>> UpdateExhibit(int id, UpdateExhibitDto updatedExhibit)
    {
        var exhibit = await _exhibitService.GetExhibitById(id);
        if (exhibit == null) return NotFound();

        if (!string.IsNullOrEmpty(updatedExhibit.Title))
        {
            exhibit.Title = updatedExhibit.Title;
        }

        if (!string.IsNullOrEmpty(updatedExhibit.Description))
        {
            exhibit.Description = updatedExhibit.Description;
        }

        if (updatedExhibit.MaxCapacity.HasValue)
        {
            exhibit.MaxCapacity = updatedExhibit.MaxCapacity.Value;
        }

        if (updatedExhibit.InitialDuration.HasValue)
        {
            exhibit.InitialDuration = updatedExhibit.InitialDuration.Value;
        }

        if (updatedExhibit.InsuranceFormRequired.HasValue)
        {
            exhibit.InsuranceFormRequired = updatedExhibit.InsuranceFormRequired.Value;
        }

        if (updatedExhibit.AgeRequired.HasValue)
        {
            exhibit.AgeRequired = updatedExhibit.AgeRequired.Value;
        }

        if (!string.IsNullOrEmpty(updatedExhibit.InsuranceFormFileUrl)) 
        {
            exhibit.InsuranceFormFileUrl = updatedExhibit.InsuranceFormFileUrl;
        }

        await _exhibitService.UpdateSingleExhibit(exhibit);
        return Ok(exhibit);
    }
}
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Application.Services;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Core.Validators;
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
            AgeMinimum = exhibit.AgeMinimum
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
        try
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<ExhibitMap>();
                exhibitsList = csv.GetRecords<Exhibit>().ToList();
            }
        }
        catch (Exception ex)
        {
            return BadRequest($"Error parsing the file: {ex.Message}");
        }


        string validationError = string.Empty;
        for (int i = 0; i < exhibitsList.Count; i++)
        {
            string singleError = exhibitsList[i].Validate();
            if (!string.IsNullOrWhiteSpace(singleError))
                validationError += $"Errors at row {i + 1}: {singleError}";
        }

        if (!string.IsNullOrWhiteSpace(validationError))
        {
            return BadRequest(validationError);
        }

        await _exhibitService.AddMultipleExhibits(exhibitsList);
        return Ok("File is in the correct format and was successfully parsed.");
    }
}
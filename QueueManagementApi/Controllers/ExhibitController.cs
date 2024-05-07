using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Core.Validators;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using QueueManagementApi.Application.Services.ExhibitService;

namespace QueueManagementApi.Controllers;

public class ExhibitController : ApiController
{
    private readonly IExhibitService _exhibitService;

    public ExhibitController(IExhibitService exhibitService)
    {
        _exhibitService = exhibitService;
    }

    [HttpGet]
    public ActionResult<List<Exhibit>> GetExhibits(int page, int pageSize)
    {
        var result = _exhibitService.GetExhibits(page, pageSize);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Exhibit>> GetExhibitById(int id)
    {
        var exhibit = await _exhibitService.GetExhibitById(id);
        if (exhibit == null) return NotFound();

        return Ok(exhibit);
    }

    [HttpPost("createSingleExhibit")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Exhibit>> CreateExhibit([FromBody] CreateExhibitDto exhibit)
    {
        var exhibitEntity = new Exhibit
        {
            Title = exhibit.Title,
            Description = exhibit.Description,
            MaxCapacity = exhibit.MaxCapacity,
            InitialDuration = exhibit.InitialDuration,
            InsuranceFormRequired = exhibit.InsuranceFormRequired,
            AgeRequired = exhibit.AgeRequired,
            AgeMinimum = exhibit.AgeMinimum,
            CategoryId = exhibit.CategoryId,
            ExhibitImages = exhibit.ExhibitImages.Select(x => 
                new ExhibitImage() { DisplayOrder = x.DisplayOrder, ImageUrl = x.ImageUrl}).ToList()
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

    [HttpPut("updateSingleExhibit")]
    public async Task<ActionResult<Exhibit>> UpdateExhibit(int id, UpdateExhibitDto updatedExhibit)
    {
        var exhibit = await _exhibitService.GetExhibitById(id);
        if (exhibit == null) return NotFound();

        var initialState = new
        {
            exhibit.Title,
            exhibit.Description,
            exhibit.MaxCapacity,
            exhibit.InitialDuration,
            exhibit.InsuranceFormRequired,
            exhibit.AgeRequired,
            exhibit.InsuranceFormFileUrl
        };

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

        var updatedState = new
        {
            exhibit.Title,
            exhibit.Description,
            exhibit.MaxCapacity,
            exhibit.InitialDuration,
            exhibit.InsuranceFormRequired,
            exhibit.AgeRequired,
            exhibit.InsuranceFormFileUrl
        };

        if (initialState.Equals(updatedState))
        {
            return BadRequest("No updates were necessary for this exhibit.");
        }

        await _exhibitService.UpdateSingleExhibit(exhibit);
        return Ok(exhibit);
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStatistics()
    {
        var exhibitCount = _exhibitService.GetExhibits(1, 1).TotalCount;
        var staffMemberCount = await _exhibitService.GetStaffMemberCount();
        int totalVisitors = await _exhibitService.GetTotalVisitors();
        List<Exhibit> topReserverExhibits = await _exhibitService.GetTopExhibits();
        return Ok(new
        {
            exhibitCount,
            staffMemberCount,
            totalVisitors,
            topReserverExhibits
        });
    }
}
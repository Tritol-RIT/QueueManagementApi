using Microsoft.AspNetCore.Mvc;
using QueueManagementApi.Core.Interfaces;

namespace QueueManagementApi.Controllers;

public class FileController : ApiController
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        await using var stream = file.OpenReadStream();

        var filePath = await _fileService.SaveFileAsync(stream, $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}");

        return Ok(new
        {
            Success = true,
            FilePath = filePath
        });
    }

}
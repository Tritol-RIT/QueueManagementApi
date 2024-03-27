using Microsoft.Extensions.Configuration;
using QueueManagementApi.Core;
using QueueManagementApi.Core.Extensions;
using QueueManagementApi.Core.Interfaces;

namespace QueueManagementApi.Infrastructure.Services.FileService;

public class LocalFileService : IFileService
{
    private readonly string _basePath;

    public LocalFileService(IConfiguration configuration)
    {
        _basePath = configuration["FileStorage:BasePath"] ?? Path.Combine(Directory.GetCurrentDirectory(), "Files");

        // ensure the files directory exists, if not create it
        if (!Directory.Exists(_basePath))
            Directory.CreateDirectory(_basePath);
    }

    public async Task SaveFileAsync(Stream fileStream, string fileName)
    {
        var filePath = Path.Combine(_basePath, fileName);
        var directoryPath = Path.GetDirectoryName(filePath);

        // Ensure the directory exists
        if (directoryPath.IsEmpty())
            throw new ArgumentException("Could not save file, file directory not set.");
        
        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(directoryPath!);

        await using var file = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        await fileStream.CopyToAsync(file);
    }

    public async Task<Stream> GetFileAsync(string fileName)
    {
        var filePath = Path.Combine(_basePath, fileName);
        return new FileStream(filePath, FileMode.Open, FileAccess.Read);
    }
}

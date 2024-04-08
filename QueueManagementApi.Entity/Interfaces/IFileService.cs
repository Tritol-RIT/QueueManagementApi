namespace QueueManagementApi.Core.Interfaces;

public interface IFileService
{
    Task<string> SaveFileAsync(Stream fileStream, string fileName);
    Task<Stream> GetFileAsync(string fileName);
    // other methods, such as delete, list?
}
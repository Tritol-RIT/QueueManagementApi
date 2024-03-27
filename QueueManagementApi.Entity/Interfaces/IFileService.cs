namespace QueueManagementApi.Core.Interfaces;

public interface IFileService
{
    Task SaveFileAsync(Stream fileStream, string fileName);
    Task<Stream> GetFileAsync(string fileName);
    // other methods, such as delete, list?
}
using QueueManagementApi.Core.Entities;

namespace QueueManagementApi.Application.Services;

public interface IExhibitService
{
    Task<Exhibit?> GetExhibitById(int id);
    IEnumerable<Exhibit> GetAllExhibits();
}
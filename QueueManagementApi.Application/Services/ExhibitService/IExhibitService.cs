using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Core.Entities;

namespace QueueManagementApi.Application.Services.ExhibitService;

public interface IExhibitService
{
    Task<Exhibit?> GetExhibitById(int id);
    IEnumerable<Exhibit> GetAllExhibits();
    Task AddSingleExhibit(Exhibit exhibit);

    Task AddMultipleExhibits(List<Exhibit> file);

    Task UpdateSingleExhibit(Exhibit exhibit);
}
using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Core.Pagination;

namespace QueueManagementApi.Application.Services.ExhibitService;

public interface IExhibitService
{
    Task<Exhibit?> GetExhibitById(int id);

    PagedList<Exhibit> GetExhibits(int page, int pageSize);
    
    IEnumerable<Exhibit> GetAllExhibits();
    
    Task AddSingleExhibit(Exhibit exhibit);

    Task AddMultipleExhibits(List<Exhibit> file);

    Task UpdateSingleExhibit(Exhibit exhibit);
}
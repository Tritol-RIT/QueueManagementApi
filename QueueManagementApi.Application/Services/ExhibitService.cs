using QueueManagementApi.Core.Entities;
using QueueManagementApi.Core.Interfaces;

namespace QueueManagementApi.Application.Services;

public class ExhibitService : IExhibitService
{
    private readonly IRepository<Exhibit> _exhibitRepository;

    public ExhibitService(IRepository<Exhibit> exhibitRepository)
    {
        _exhibitRepository = exhibitRepository;
    }

    public async Task<Exhibit?> GetExhibitById(int id)
    {
        return await _exhibitRepository.FindById(id);
    }

    public IEnumerable<Exhibit> GetAllExhibits()
    {
        return _exhibitRepository.GetAll().ToList();
    }
}
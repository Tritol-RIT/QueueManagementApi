using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Core.Interfaces;

namespace QueueManagementApi.Application.Services;

public class ExhibitService : IExhibitService
{
    private readonly IRepository<Exhibit> _exhibitRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ExhibitService(IRepository<Exhibit> exhibitRepository, IUnitOfWork unitOfWork)
    {
        _exhibitRepository = exhibitRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Exhibit?> GetExhibitById(int id)
    {
        return await _exhibitRepository.FindById(id);
    }

    public IEnumerable<Exhibit> GetAllExhibits()
    {
        return _exhibitRepository.GetAll().ToList();
    }
    public async Task AddSingleExhibit(Exhibit exhibit)
    {
        await _exhibitRepository.AddAsync(exhibit);
        await _unitOfWork.CompleteAsync();
    }

    public async Task AddMultipleExhibits(List<Exhibit> file)
    {
        foreach (var exhibit in file)
        {
            await _exhibitRepository.AddAsync(exhibit);
        }
        await _unitOfWork.CompleteAsync();
    }

    public async Task UpdateSingleExhibit(Exhibit exhibit)
    {
        _exhibitRepository.Update(exhibit);
        await _unitOfWork.CompleteAsync();
    }
}
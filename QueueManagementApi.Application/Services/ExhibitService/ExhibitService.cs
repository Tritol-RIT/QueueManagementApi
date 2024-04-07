using Microsoft.EntityFrameworkCore;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Core.Interfaces;
using QueueManagementApi.Core.Pagination;

namespace QueueManagementApi.Application.Services.ExhibitService;

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

    public PagedList<Exhibit> GetExhibits(int page, int pageSize)
    {
        var query = _exhibitRepository.GetAll()
            .Include(x => x.Category)
            .Include(x => x.ExhibitImages);

        return query.ToPagedList(page, pageSize);
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
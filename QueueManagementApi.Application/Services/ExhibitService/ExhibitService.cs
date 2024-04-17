using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Core.Enums;
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

    public async Task<int> GetStaffMemberCount()
    {
        return await _unitOfWork.Repository<User>().GetAll().Where(x => x.Role == UserRole.Staff).CountAsync();
    }

    public async Task<int> GetTotalVisitors()
    {
        return await _unitOfWork.Repository<Visitor>().GetAll().CountAsync();
    }

    public async Task<List<Exhibit>> GetTopExhibits()
    {
        var query =
                _exhibitRepository
                    .GetAll()
                    .Include(x => x.Category)
                    .Include(x => x.Visits)
                    .ThenInclude(x => x.Group)
                    .ThenInclude(x => x.Visitors);

        var query2 = query.OrderByDescending(x => x.Visits.Count());

        return await query2.Take(3).ToListAsync();
    }
}
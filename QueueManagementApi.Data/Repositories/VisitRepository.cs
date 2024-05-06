using Microsoft.EntityFrameworkCore;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Core.Interfaces;

namespace QueueManagementApi.Infrastructure.Repositories;

public class VisitRepository : GenericRepository<Visit>, IVisitRepository
{
    public VisitRepository(DbContext context) : base(context) {}

    public async Task<List<Visit>> GetVisitsByExhibitId(int exhibitId)
    {
        return await _context.Set<Visit>()
            .Include(v => v.Exhibit)
            .Where(v => v.ExhibitId == exhibitId)
            .OrderBy(v => v.PotentialStartTime)
            .ToListAsync();
    }

    public async Task<List<Visit>> GetVisitsByVisitorEmail(string visitorEmail)
    {
        return await _context.Set<Visit>()
            .Include(v => v.Exhibit)
            .Where(v => v.Group.Visitors.Any(visitor => visitor.Email == visitorEmail))
            .OrderBy(v => v.PotentialStartTime)
            .ToListAsync();
    }
    public async Task<List<Visit>> GetVisits()
    {
        return await _context.Set<Visit>()
            .Include(v => v.Group)
            .Include(v => v.Group.Visitors)
            .Include(v => v.Exhibit)
            .ToListAsync();
    }
}
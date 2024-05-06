using QueueManagementApi.Core.Entities;

namespace QueueManagementApi.Core.Interfaces;

public interface IVisitRepository : IRepository<Visit>
{
    Task<List<Visit>> GetVisitsByExhibitId(int exhibitId);
    Task<List<Visit>> GetVisitsByVisitorEmail(string visitorEmail);
    Task<List<Visit>> GetVisits();
}
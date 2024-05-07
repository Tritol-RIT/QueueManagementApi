using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Core;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Core.Interfaces;


namespace QueueManagementApi.Application.Services.CheckinService;
public class CheckinService : ICheckinService
{
    private readonly IRepository<Group> _groupRepository;
    private readonly IVisitRepository _visitRepository;
    private readonly IRepository<Exhibit> _exhibitRepository;
    private readonly IRepository<Visitor> _visitorRepository;
    private readonly IRepository<Insurance> _insuranceRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CheckinService(IVisitRepository visitRepository, IUnitOfWork unitOfWork, IRepository<Group> groupRepository, IRepository<Exhibit> exhibitRepository, IRepository<Visitor> visitorRepository, IRepository<Insurance> insuranceRepository)
    {
        _visitRepository = visitRepository;
        _unitOfWork = unitOfWork;
        _groupRepository = groupRepository;
        _exhibitRepository = exhibitRepository;
        _visitorRepository = visitorRepository;
        _insuranceRepository = insuranceRepository;
    }
    public async Task<int> StartCheckin(int exhibitId, string QrCodeGUID)
    {
        var firstVisitInQueue = _visitRepository.FindByCondition(x => x.ExhibitId == exhibitId && x.ActualStartTime == null).OrderBy(x => x.ActualStartTime).FirstOrDefault();
        var visit = _visitRepository.FindByCondition(x => x.QrCode == QrCodeGUID).FirstOrDefault();
        if (visit == null)
            throw new QueueApiException($"Visit with Id {QrCodeGUID} does not exist");
        else if (visit.QrCode == firstVisitInQueue.QrCode)
        {
            var nrOfVisitors = _groupRepository.FindByCondition(x => x.Id == visit.GroupId).FirstOrDefault().NumberOfVisitors;
            visit.ActualStartTime = DateTime.UtcNow;
            _visitRepository.Update(visit);
            return nrOfVisitors;
        }
        return 0;
    }
    public async Task<bool> insuranceFind(int exhibitId)
    {
        var insurance = _exhibitRepository.FindByCondition(x => x.Id == exhibitId).FirstOrDefault();
        return insurance.InsuranceFormRequired;
    }

    public async Task updateVisit(InsuranceDto item, string QrCodeGUID)
    {
        var visit = _visitRepository.GetVisitByQrCode(QrCodeGUID).Result;
        if (visit == null)
            throw new QueueApiException($"Visit with Id {QrCodeGUID} does not exist");
        else
        {
            try
            {
                Visitor newVisit = new Visitor
                {
                    CreatedOn = DateTime.UtcNow,
                    Email = item.Email,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    Group = visit.Group,
                    GroupId = visit.GroupId,
                };
                Insurance newInsurance = new Insurance
                {
                    ApprovalTime = DateTime.UtcNow,
                    VisitorId = newVisit.Id,
                    VisitorImageUrl = item.imageUrl,
                };

                await _visitorRepository.AddAsync(newVisit);
                await _insuranceRepository.AddAsync(newInsurance);
            }
            catch (Exception)
            {
                throw new QueueApiException($"Failed to save");
            }            
        }
    }
    public void checkOut()
    {
        var lastVisit = _visitRepository.FindByCondition(x => x.ActualEndTime == null && x.ActualStartTime !=null).OrderBy(x => x.ActualStartTime).FirstOrDefault();
        lastVisit.ActualEndTime = DateTime.UtcNow;
        _visitRepository.Update(lastVisit);
    }
}


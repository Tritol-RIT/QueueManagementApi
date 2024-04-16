using QueueManagementApi.Core.Entities;

namespace QueueManagementApi.Core.Services.WaitTimeCalculationService;

public interface IWaitTimeCalculationService
{
    Task<(DateTime potentialStartTime, DateTime potentialEndTime)> CalculatePotentialTimes(Exhibit exhibit, string visitorEmail);
}
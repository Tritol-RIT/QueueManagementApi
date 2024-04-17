using Microsoft.EntityFrameworkCore;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Core.Interfaces;
using QueueManagementApi.Core.Services.WaitTimeCalculationService;

namespace QueueManagementApi.Application.Services.WaitTimeCalculationService;

public class WaitTimeCalculationService : IWaitTimeCalculationService
{
    private readonly IVisitRepository _visitRepository;

    public WaitTimeCalculationService(IVisitRepository visitRepository)
    {
        _visitRepository = visitRepository;
    }

    public async Task<(DateTime potentialStartTime, DateTime potentialEndTime)> CalculatePotentialTimes(Exhibit exhibit, string visitorEmail)
    {
        var visitDuration = exhibit.CurrentDuration ?? exhibit.InitialDuration;

        var allExhibitVisits = await _visitRepository.GetVisitsByExhibitId(exhibit.Id);
        var allVisitorVisits = await _visitRepository.GetVisitsByVisitorEmail(visitorEmail);

        DateTime potentialStartTime = DateTime.UtcNow;
        DateTime potentialEndTime;

        // First, try to find a gap in the exhibit schedule
        for (int i = 0; i <= allExhibitVisits.Count; i++)
        {
            var previousVisitEndTime = i == 0 ? DateTime.UtcNow : allExhibitVisits[i - 1].PotentialEndTime;
            var nextVisitStartTime = i == allExhibitVisits.Count ? DateTime.MinValue : allExhibitVisits[i].PotentialStartTime;

            // Check if the gap is big enough for the new visit
            if (nextVisitStartTime >= previousVisitEndTime.AddMinutes(visitDuration))
            {
                // Proposed new times
                potentialStartTime = previousVisitEndTime.AddMinutes(2); // Start immediately after the previous visit ends
                potentialEndTime = potentialStartTime.AddMinutes(visitDuration);

                // Ensure no overlap with other visits by the same visitor
                if (!allVisitorVisits.Any(v =>
                      (v.PotentialStartTime <= potentialStartTime && v.PotentialEndTime > potentialStartTime) || // Overlaps at the beginning
                      (v.PotentialStartTime < potentialEndTime && v.PotentialEndTime >= potentialEndTime) || // Overlaps at the end
                      (potentialStartTime > v.PotentialStartTime && potentialEndTime < v.PotentialEndTime) // Completely within another visit
                    ))
                {
                    return (potentialStartTime, potentialEndTime);
                }
            }
        }

        // If no suitable gap is found, find the earliest possible time considering both the visitor's and exhibit's schedules
        DateTime latestExhibitTime = allExhibitVisits.LastOrDefault()?.PotentialEndTime ?? DateTime.UtcNow;
        DateTime latestVisitorTime = allVisitorVisits.LastOrDefault()?.PotentialEndTime ?? DateTime.UtcNow;

        // Schedule at the earliest possible time after the latest of the visitor's and exhibit's commitments
        potentialStartTime = latestExhibitTime > latestVisitorTime ? latestExhibitTime : latestVisitorTime;

        // Calculate the ideal start time considering exhibit duration as a slot
        var minutesToAdd = (potentialStartTime - latestExhibitTime).TotalMinutes % visitDuration;

        // If there's enough time for at least one full slot after the latest exhibit visit
        if (minutesToAdd > 0 && latestExhibitTime > DateTime.UtcNow.AddMinutes(visitDuration))
        {
            potentialStartTime = potentialStartTime.AddMinutes(minutesToAdd); // Move to the next slot
        }
        else
        {
            potentialStartTime = potentialStartTime.AddMinutes(2); // Default behavior if not enough time for a full slot
        }

        potentialEndTime = potentialStartTime.AddMinutes(visitDuration);
        return (potentialStartTime, potentialEndTime);
    }
}
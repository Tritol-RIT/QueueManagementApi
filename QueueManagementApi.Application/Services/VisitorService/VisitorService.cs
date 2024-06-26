﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Application.Services.ExhibitService;
using QueueManagementApi.Application.Services.QrCodeService;
using QueueManagementApi.Application.Services.EmailService;
using QueueManagementApi.Core;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Core.Extensions;
using QueueManagementApi.Core.Interfaces;
using QueueManagementApi.Core.Services.WaitTimeCalculationService;
using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Core.Pagination;

namespace QueueManagementApi.Application.Services.VisitorService;

public class VisitorService : IVisitorService
{
    private readonly ILogger<VisitorService> _logger;
    private readonly IExhibitService _exhibitService;
    private readonly IQrCodeService _qrCodeService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Group> _groupRepository;
    private readonly IVisitRepository _visitRepository;
    private readonly IRepository<Visitor> _visitorRepository;
    private readonly IWaitTimeCalculationService _waitTimeCalculationService;
    private readonly IEmailService _emailService;

    public VisitorService(
        ILogger<VisitorService> logger, 
        IExhibitService exhibitService, 
        IQrCodeService qrCodeService, 
        IUnitOfWork unitOfWork, 
        IWaitTimeCalculationService waitTimeCalculationService, 
        IVisitRepository visitRepository,
        IEmailService emailService)
    {
        _logger = logger;
        _exhibitService = exhibitService;
        _qrCodeService = qrCodeService;
        _unitOfWork = unitOfWork;
        _waitTimeCalculationService = waitTimeCalculationService;
        _visitRepository = visitRepository;
        _emailService = emailService;

        _groupRepository = unitOfWork.Repository<Group>();
        _visitorRepository = unitOfWork.Repository<Visitor>();
    }

    public async Task<Visit> Register(RegisterVisitorDto registerVisitorDto)
    {
        var exhibit = await _exhibitService.GetExhibitById(registerVisitorDto.ExhibitId);
        if (exhibit == null)
            throw new QueueApiException($"Exhibit with Id {registerVisitorDto.ExhibitId} does not exist");

        DateTime potentialStartTime;
        DateTime potentialEndTime;
        try
        {
            (potentialStartTime, potentialEndTime) =
                await _waitTimeCalculationService.CalculatePotentialTimes(exhibit, registerVisitorDto.Email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Wait time calculation failed: {ex.Message}");

            potentialStartTime = ((await _visitRepository.GetVisitsByExhibitId(exhibit.Id)).LastOrDefault()?.PotentialStartTime ?? DateTime.UtcNow).AddMinutes(2);
            potentialEndTime = potentialStartTime.AddMinutes(exhibit.CurrentDuration ?? exhibit.InitialDuration);
        }

        var group = new Group
        {
            NumberOfVisitors = registerVisitorDto.NumberOfPeople,
            VisitDate = potentialStartTime
        };

        var initialVisitor = new Visitor
        {
            Email = registerVisitorDto.Email,
            FirstName = registerVisitorDto.FirstName,
            LastName = registerVisitorDto.LastName,
            Group = group
        };

        var visit = new Visit
        {
            Group = group,
            Exhibit = exhibit,
            QrCode = $"{Guid.NewGuid()}",
            PotentialEndTime = potentialEndTime,
            PotentialStartTime = potentialStartTime
        };

        await _groupRepository.AddAsync(group);
        await _visitorRepository.AddAsync(initialVisitor);
        await _visitRepository.AddAsync(visit);

        await _unitOfWork.CompleteAsync();

        // Send email with QR Code
        await _emailService.SendVisitorRegistrationEmailAsync(initialVisitor.Email, "Exhibit Registration Confirmation", visit, initialVisitor);

        return visit;
    }
    public async Task<PagedList<AllVisitDto>> VisitGetAll(int page, int pageSize, string? search)
    {
        List<Visit> allVisit = await _visitRepository.GetVisits();
        var allVisitDtos = allVisit
            .GroupBy(v => v.Group.Visitors.FirstOrDefault().Email)
            .Select(g => new AllVisitDto
            {
                VisitorEmail = g.Key,
                VisitorFirstName = g.First().Group.Visitors.FirstOrDefault().FirstName,
                VisitorLastName = g.First().Group.Visitors.FirstOrDefault().LastName,
                nrOfRegisteredVisits = g.Count(),
                lastExhibitName = g.Last().Exhibit.Title,
            });

        if (!search.IsEmpty())
            allVisitDtos = allVisitDtos.Where(x =>
                x.VisitorEmail.Contains(search) || x.VisitorFirstName.Contains(search) ||
                x.VisitorLastName.Contains(search));

        return allVisitDtos.ToPagedList(page, pageSize);
    }
}
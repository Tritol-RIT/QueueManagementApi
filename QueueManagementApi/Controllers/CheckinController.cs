using Microsoft.AspNetCore.Mvc;
using QueueManagementApi.Application.Dtos;
using QueueManagementApi.Application.Services.CheckinService;
using QueueManagementApi.Core.Entities;

namespace QueueManagementApi.Controllers;

[Route("checkin")]
public class CheckinController : ApiController
{
    private readonly ICheckinService _checkinService;

    public CheckinController(ICheckinService checkinService)
    {
        _checkinService = checkinService;
    }

    [HttpGet("StartCheckin")]
    public ActionResult StartCheckin(int exhibitId, string QrCodeGUID)
    {
        int nrOfPeople = _checkinService.StartCheckin(exhibitId, QrCodeGUID).Result;
        bool insuranceForm = _checkinService.insuranceFind(exhibitId).Result;
        var res = $"{{nrOfPeople: {nrOfPeople}, insuranceForm: {insuranceForm}}}";
        return Ok(res);
    }
    [HttpPost("InsuranceRecord")]
    public ActionResult RecordInsurance(List<InsuranceDto> listInfo, string QrCodeGUID)
    {
        foreach (InsuranceDto item in listInfo)
        {
            _checkinService.updateVisit(item,QrCodeGUID);
        }
        return Ok();
    }
    [HttpPost("CheckOut")]
    public ActionResult checkOutLastVisitor()
    {
        _checkinService.checkOut();
        return Ok();
    }
}
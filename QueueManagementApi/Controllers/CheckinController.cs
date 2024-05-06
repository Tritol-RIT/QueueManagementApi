using Microsoft.AspNetCore.Mvc;
using QueueManagementApi.Core.Entities;
using QueueManagementApi.Application.Services.CheckinService;
using QueueManagementApi.Application.Services.QrCodeService;

namespace QueueManagementApi.Controllers;

[Route("checkin")]
public class CheckinController : ApiController
{

    public CheckinController()
    {
    }
    [HttpGet("StartCheckin")]
    public ActionResult<List<Exhibit>> StartCheckin(int exhibitId,string QrCodeGUID)
    {
        return Ok();
    }

}
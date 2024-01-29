using Microsoft.AspNetCore.Mvc;

namespace QueueManagementApi.Controllers;

public class HealthController : ControllerBase
{
    [HttpGet("/healthcheck")]
    public ActionResult Healthcheck() => Ok(new { Healthy = true });
}
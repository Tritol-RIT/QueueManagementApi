using Microsoft.AspNetCore.Mvc;

namespace QueueManagementApi.Controllers;

public class HealthController : ApiController
{
    [HttpGet("healthcheck")]
    public ActionResult Healthcheck() => Ok(new { Healthy = true });
}
using Microsoft.AspNetCore.Mvc;
using QueueManagementApi.Application.Services;
using QueueManagementApi.Application.Services.EmailService;
using QueueManagementApi.Core.Entities;

namespace QueueManagementApi.Controllers;

[Route("email")]
public class EmailController : ApiController
{
    private readonly IEmailService _emailService;
    public EmailController(IEmailService emailService)
    {
        _emailService = emailService;
    }
    // POST /email/send
    [HttpPost("send")]
    public async Task<ActionResult> SendEmail(string email, string subject, string message)
    {
        await _emailService.SendEmailAsync(email, subject, message);
        return Ok();
    }
}

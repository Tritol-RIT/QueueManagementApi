using Microsoft.AspNetCore.Mvc;
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
    [HttpPost("sendUserEmail")]
    public async Task<ActionResult> SendEmailUser(string email, string subject, User user)
    {
        await _emailService.SendEmailUserAsync(email, subject, user, "test");
        return Ok();
    }
    [HttpPost("sendEmail")]
    public async Task<ActionResult> SendEmailAsync(string email, string subject, string body)
    {
        await _emailService.SendEmailAsync(email, subject, body);
        return Ok();
    }
}
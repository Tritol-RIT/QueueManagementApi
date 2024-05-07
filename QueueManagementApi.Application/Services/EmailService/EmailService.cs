using QueueManagementApi.Core.Entities;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using QueueManagementApi.Application.Dtos;
using System.Globalization;
using QueueManagementApi.Application.Services.QrCodeService;

namespace QueueManagementApi.Application.Services.EmailService;


public class EmailService : IEmailService
{
    private const string _smtpServer = "smtp.gmail.com";
    private const int _smtpPort = 587;
    private const string _smtpUsername = "denivetoni@gmail.com";
    private const string _smtpPassword = "rgapduuvyqjzghkk";
    private const string relativePathAttachment = @"..\..\..\..\QueueManagementApi.Application\EmailTemplates\test.txt";
    
    private readonly IEmailTemplateRenderer _emailTemplate;
    private readonly IConfiguration _configuration;
    private readonly IQrCodeService _qrCodeService;

    public EmailService(IEmailTemplateRenderer emailTemplate, IConfiguration configuration, IQrCodeService qrCodeService)
    {
        _emailTemplate = emailTemplate;
        _configuration = configuration;
        _qrCodeService = qrCodeService;
    }

    public async Task SendEmailAsync(string email, string subject, string body)
    {

        string attachmentFilePath = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), relativePathAttachment);
        using (var client = new SmtpClient())
        {
            client.Host = _smtpServer;
            client.Port = _smtpPort;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
            using (var message = new MailMessage(
                from: new MailAddress(_smtpUsername, "RITK Queue Management"),
                to: new MailAddress(email, "Visitor")
                ))
            {

                message.Subject = subject;
                message.Body = body;

                var attachment = new Attachment(attachmentFilePath);
                message.Attachments.Add(attachment);

                await client.SendMailAsync(message);
            }
        }
    }

    public async Task SendEmailUserAsync(string email, string subject, User model, string setPasswordToken)
    {
        var applicationDomain = _configuration.GetValue<string>("ApplicationDomain");
        try
        {
            using var client = new SmtpClient();
            client.Host = _smtpServer;
            client.Port = _smtpPort;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);


            using var message = new MailMessage(
                from: new MailAddress(_smtpUsername, "RITK Queue Management"),
                to: new MailAddress(email, $"{model.FirstName} {model.LastName}"));

            message.IsBodyHtml = true;
            message.Subject = subject;

            var emailModel = new CreateUserEmailDto
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordSetTokenLink = $"{applicationDomain}/set-password/{setPasswordToken}",
                Role = model.Role
            };

            var emailBody = await _emailTemplate.RenderEmailTemplateAsync("UserCreationEmailTemplate.cshtml", emailModel);

            message.Body = emailBody;

            await client.SendMailAsync(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task SendUserResetPasswordEmailAsync(string email, string subject, User model, string resetPasswordToken)
    {
        var applicationDomain = _configuration.GetValue<string>("ApplicationDomain");
        try
        {
            using var client = new SmtpClient();
            client.Host = _smtpServer;
            client.Port = _smtpPort;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);


            using var message = new MailMessage(
                from: new MailAddress(_smtpUsername, "RITK Queue Management"),
                to: new MailAddress(email, $"{model.FirstName} {model.LastName}"));

            message.IsBodyHtml = true;
            message.Subject = subject;

            var emailModel = new ResetPasswordEmailDto
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordResetTokenLink = $"{applicationDomain}/set-password/{resetPasswordToken}"
            };

            var emailBody = await _emailTemplate.RenderEmailTemplateAsync("ResetPasswordEmailTemplate.cshtml", emailModel);

            message.Body = emailBody;

            await client.SendMailAsync(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task SendVisitorRegistrationEmailAsync(string email, string subject, Visit visit, Visitor visitor)
    {
        try
        {
            using var client = new SmtpClient();
            client.Host = _smtpServer;
            client.Port = _smtpPort;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);

            using var message = new MailMessage(
                from: new MailAddress(_smtpUsername, "RITK Queue Management"),
                to: new MailAddress(email, $"{visitor.FirstName} {visitor.LastName}"));

            message.IsBodyHtml = true;
            message.Subject = subject;

            var emailModel = new RegisterVisitorEmailDto
            {
                FirstName = visitor.FirstName,
                LastName = visitor.LastName,
                ExhibitTitle = visit.Exhibit.Title,
                QrCodeImage = $"https://api.qrserver.com/v1/create-qr-code/?size=150x150&data={visit.QrCode}",
                VisitDate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(visit.PotentialStartTime.AddHours(-1), "GTB Standard Time").ToString("g")
            };

            var emailBody = await _emailTemplate.RenderEmailTemplateAsync("RegisterVisitorEmailTemplate.cshtml", emailModel);

            message.Body = emailBody;

            await client.SendMailAsync(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}


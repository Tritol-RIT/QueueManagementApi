﻿using QueueManagementApi.Core.Entities;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Razor;
using RazorLight;
using System.Reflection;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using QRCoder;
using QueueManagementApi.Application.Dtos;

namespace QueueManagementApi.Application.Services.EmailService;


public class EmailService : IEmailService
{
    private const string _smtpServer = "smtp.gmail.com";
    private const int _smtpPort = 587;
    private const string _smtpUsername = "denivetoni@gmail.com";
    private const string _smtpPassword = "rgapduuvyqjzghkk";
    private const string relativePathAttachment = @"..\..\..\..\QueueManagementApi.Application\EmailTemplates\test.txt";

    private const string relativePathTemplate = @"..\..\..\..\QueueManagementApi.Application\EmailTemplates\UserCreationEmailTemplate.cshtml";

    private readonly IEmailTemplateRenderer _emailTemplate;
    private readonly IConfiguration _configuration;

    public EmailService(IEmailTemplateRenderer emailTemplate, IConfiguration configuration)
    {
        _emailTemplate = emailTemplate;
        _configuration = configuration;
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
                from: new MailAddress(_smtpUsername, "RITK Queue Managment"),
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
        string templatePath = Path.Combine(Path.GetDirectoryName(Environment.ProcessPath),relativePathTemplate);
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

            var emailBody = await _emailTemplate.RenderEmailTemplateAsync(templatePath, emailModel);

            message.Body = emailBody;

            await client.SendMailAsync(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}


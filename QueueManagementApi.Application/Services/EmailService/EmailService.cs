using System.Net.Mail;
using System.Net;
namespace QueueManagementApi.Application.Services.EmailService;


public class EmailService : IEmailService
{
    private const string _smtpServer = "smtp.gmail.com";
    private const int _smtpPort = 587;
    private const string _smtpUsername = "denivetoni@gmail.com";
    private const string _smtpPassword = "rgapduuvyqjzghkk";
    private const string attachmentFilePath = "C:\\Users\\vetonk\\Downloads\\test.txt";
    public async Task SendEmailAsync(string email, string subject, string body)
    {
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
}


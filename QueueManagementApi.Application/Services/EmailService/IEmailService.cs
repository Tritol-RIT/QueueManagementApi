using QueueManagementApi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueManagementApi.Application.Services.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string body);
        Task SendEmailUserAsync(string email, string subject, User model, string rawPassword);
        Task SendUserResetPasswordEmailAsync(string email, string subject, User model, string resetPasswordToken);
        Task SendVisitorRegistrationEmailAsync(string email, string subject, Visit visit, Visitor visitor);
    }
}

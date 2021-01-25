using EGPS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string subject, string body, string recipient, Notification notification);
        Task<List<string>> SendMultipleEmailsAsync(string subject, IDictionary<string, string> body, IList<Notification> notifications);
    }
}

using EGPS.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface IEmailLogger
    {
        Task LogEmailAsync(Notification notification);
        Task LogMultipleEmailAsync(IList<Notification> notifications);
    }
}

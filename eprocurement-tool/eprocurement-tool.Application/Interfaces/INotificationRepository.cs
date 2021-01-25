using EGPS.Application.Models;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EGPS.Application.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<PagedList<Notification>> GetNotifications(Guid CurrentUserId, NotificationParameters parameters);
        Task<IEnumerable<Notification>> MarkAsRead(IEnumerable<Guid> NotificationIds);
        public Task LogProcurementNotification(Guid procurementActivityId);
        public Task LogAcceptanceLetterResponseNotification(Guid contractId, EAcceptanceLetterResponse response);
        public Task LogBidSubmissioNotice(Guid procurementId, Guid vendorId);
        public Task LogContractSigningNotice(Guid procurementId);
    }
}

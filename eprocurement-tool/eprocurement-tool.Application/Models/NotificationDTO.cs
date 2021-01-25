using EGPS.Application.Common;
using EGPS.Domain.Entities;
using System;

namespace EGPS.Application.Models
{
    public class NotificationDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string Recipient { get; set; }
        public bool IsRead { get; set; } = false;
        public string Type { get; set; }
        public string NotificationType { get; set; }
        public string TemplateId { get; set; }
        public string Status { get; set; } = "SUCCESS";
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
        
    }

    public class NotificationModel
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public bool IsRead { get; set; }
        public string NotificationClass { get; set; }
        public string ActionText { get; set; }
        public Guid ActionId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

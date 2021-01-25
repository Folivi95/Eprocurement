using EGPS.Domain.Common;
using EGPS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EGPS.Domain.Entities
{
    public class Notification : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
        public string Recipient { get; set; }
        public bool IsRead { get; set; } = false;
        public EType Type { get; set; }
        public ENotificationType NotificationType { get; set; }
        public string TemplateId { get; set; }
        public string Status { get; set; } = "SUCCESS";
              
        public Guid UserId { get; set; }
        public Guid AccountId { get; set; }
        public string ActionText { get; set; }
        public Guid ActionId { get; set; }
        public ENotificationClass NotificationClass { get; set; }
    }
}

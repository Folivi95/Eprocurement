using System;

namespace EGPS.Application.Models
{
    public class NotificationForCreationDTO
    {
        public string Body { get; set; }
        public string Subject { get; set; }
        public string Recipient { get; set; }
        public string Type { get; set; }
        public string NotificationType { get; set; }
        public string TemplateId { get; set; }
        public string Status { get; set; } = "SUCCESS";
        public Guid AccountId { get; set; }
    }
}

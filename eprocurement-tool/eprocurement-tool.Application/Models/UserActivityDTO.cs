using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class UserActivityDTO
    {
        public Guid Id { get; set; }
        public string EventType { get; set; }
        public string ObjectClass { get; set; }
        public Guid ObjectId { get; set; }
        public Guid UserId { get; set; }
        public Guid AccountId { get; set; }
        public string Details { get; set; }
        public string IpAddress { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UserActivitiesDTO
    {
        public Guid Id { get; set; }
        public string EventType { get; set; }
        public string ObjectClass { get; set; }
        public Guid ObjectId { get; set; }
        public string Details { get; set; }
        public string IpAddress { get; set; }
        public Guid UserId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedAt { get; set; }
        public AuditUserDTO User { get; set; }
    }

    public class AuditUserDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public object ProfilePicture { get; set; }
    }
}

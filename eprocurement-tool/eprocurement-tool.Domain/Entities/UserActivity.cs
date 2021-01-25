using System;

namespace EGPS.Domain.Entities
{
    public class UserActivity
    {
        public Guid Id { get; set; }
        public string EventType { get; set; }
        public string ObjectClass { get; set; }
        public Guid ObjectId { get; set; }
        public string Details { get; set; }
        public string IpAddress { get; set; }
        public Guid UserId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public User User { get; set; }
    }
}

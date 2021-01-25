using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class UserActivityForCreationDTO
    {
        public string EventType { get; set; }
        public string ObjectClass { get; set; }
        public Guid ObjectId { get; set; }
        public Guid UserId { get; set; }
        public Guid AccountId { get; set; }
        public string Details { get; set; }
        public string IpAddress { get; set; }
    }
}

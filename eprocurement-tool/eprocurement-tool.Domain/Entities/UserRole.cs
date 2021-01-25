using System;
using EGPS.Domain.Common;

namespace EGPS.Domain.Entities
{
    public class UserRole : AuditableEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}

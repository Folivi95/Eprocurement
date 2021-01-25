using EGPS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Domain.Entities
{
    public class RoleResource : AuditableEntity
    {
        public Guid Id { get; set; }
        public Guid ResourceId { get; set; }
        public Guid RoleId { get; set; }
        public string Permissions { get; set; }
        public Resource Resource { get; set; }
    }
}

using System;
using EGPS.Domain.Common;

namespace EGPS.Domain.Entities
{
    public class BidType : AuditableEntity
    {
        public Guid Id { get; set; }
        public Guid Title { get; set; }
        public string Description { get; set; }
        public Guid CreatedById { get; set; }
        public bool Deleted { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}

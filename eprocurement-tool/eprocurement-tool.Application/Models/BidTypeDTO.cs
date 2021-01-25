using System;
using EGPS.Application.Common;

namespace EGPS.Application.Models
{
    public class BidTypeDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public Guid Title { get; set; }
        public string Description { get; set; }
        public Guid CreatedById { get; set; }
        public bool Deleted { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}

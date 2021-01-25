using System;
using EGPS.Domain.Common;
using EGPS.Domain.Enums;

namespace EGPS.Domain.Entities
{
    public class Bid : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid BidTypeId { get; set; }
        public EBidStatus Status  { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ClarificationDeadline { get; set; }
        public Guid CreatedById { get; set; }
        public bool Deleted { get; set; }
        public DateTime DeletedAt { get; set; }

        public Guid ProcurementProcessId { get; set; }

        //navigational properties
        public ProcurementProcess ProcurementProcess { get; set; }
    }
}

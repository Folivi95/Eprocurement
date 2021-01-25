using System;
using EGPS.Domain.Enums;

namespace EGPS.Application.Models
{
    public class BidForCreationDTO
    {
        public Guid Title { get; set; }
        public string Description { get; set; }
        public Guid BidTypeId { get; set; }
        public EBidStatus Status  { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ClarificationDeadline { get; set; }
        public Guid CreatedById { get; set; }
        public bool Deleted { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}

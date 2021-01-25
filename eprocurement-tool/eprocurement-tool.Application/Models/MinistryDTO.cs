using System;
using EGPS.Application.Common;

namespace EGPS.Application.Models
{
    public class MinistryDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public Guid CreatedById { get; set; }
        public string Code { get; set; }
        public Guid EstimatedValueId { get; set; }
        public Guid BidLowerThanId { get; set; }
        public int TotalBids { get; set; }
        public double EstimatedValue { get; set; }
    }
}

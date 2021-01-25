using System;
using EGPS.Application.Common;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EGPS.Application.Models
{
    public class BidDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public Guid Title { get; set; }
        public string Description { get; set; }
        public Guid BidTypeId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EBidStatus Status  { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ClarificationDeadline { get; set; }
        public Guid CreatedById { get; set; }
        public ProcurementProcessDTO ProcurementProcess { get; set; }
    }

    public class BidSummaryDTO
    {
        public int Total { get; set; }
        public int Approved { get; set; }
        public int Processing { get; set; }
        public int Rejected { get; set; }
        public int NotStarted { get; set; }
    }
}

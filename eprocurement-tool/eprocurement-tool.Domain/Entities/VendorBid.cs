using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Domain.Common;
using EGPS.Domain.Enums;

namespace EGPS.Domain.Entities
{
    public class VendorBid : AuditableEntity
    {
        public Guid Id { get; set; }
        public Guid VendorId { get; set; }
        public Guid ProcurementPlanId { get; set; } 
        public string Reason { get; set; }
        public string Currency { get; set; }
        public decimal? Amount { get; set; }
        public int Position { get; set; }
        public decimal? BidPrice { get; set; }
        public decimal? EvaluatedPrice { get; set; }
        public EVendorContractStatus? Type { get; set; } = EVendorContractStatus.INTERESTED;
        public string Ministry { get; set; }
        public string ProcurementCategory { get; set; }
        public string ProcurementType { get; set; }
        public DateTime? ExpiryDate { get; set; }

        //navigational properties
        public User Vendor { get; set; }
        public ProcurementPlan ProcurementPlan { get; set; }
    }
}

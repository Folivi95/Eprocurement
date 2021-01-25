using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Domain.Common;
using EGPS.Domain.Enums;

namespace EGPS.Domain.Entities
{
    public class VendorProcurement : AuditableEntity
    {
        public string Reason { get; set; }
        public string Currency { get; set; }
        public decimal? Amount { get; set; }
        public decimal? BidPrice { get; set; }
        public decimal? EvaluatedPrice { get; set; }
        public EVendorContractStatus? Type { get; set; }
        public int Position { get; set; }
        public Guid ProcurementPlanId { get; set; }
        public ProcurementPlan ProcurementPlan { get; set; }
        public Guid VendorId { get; set; }
        public User Vendor { get; set; }
    }
}

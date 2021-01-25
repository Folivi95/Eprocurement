using EGPS.Domain.Common;
using EGPS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Domain.Entities
{
    public class MilestoneInvoice : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime? DueDate { get; set; }
        public string InvoiceNumber { get; set; }
        public EPaymentStatus PaymentStatus { get; set; } = EPaymentStatus.PENDING;
        public DateTime? PaymentDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? DeclinedDate { get; set; }
        public EInvoiceStatus Status { get; set; } = EInvoiceStatus.PAID;
        public Guid ProjectMileStoneId { get; set; }

        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public string UniqueId { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        //navigational property
        public ProjectMileStone ProjectMileStone { get; set; }
    }
}

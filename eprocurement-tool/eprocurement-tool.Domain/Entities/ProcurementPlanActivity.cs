using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Domain.Common;
using EGPS.Domain.Enums;

namespace EGPS.Domain.Entities
{
    public class ProcurementPlanActivity : AuditableEntity
    {
        
        public Guid Id { get; set; }
        public string Title { get; set; }
        public EPprocurementPlanTask ProcurementPlanType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Index { get; set; }
        public EProcurementPlanActivityStatus ProcurementPlanActivityStatus { get; set; } = EProcurementPlanActivityStatus.INACTIVE;
        public Guid ProcurementPlanId { get; set; }
        public ProcurementPlan ProcurementPlan { get; set; }
        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public ICollection<ProcurementPlanDocument> ProcurementPlanDocuments { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public DateTime? RevisedDate { get; set; }
        public DateTime? ActualDate { get; set; }
    }
}

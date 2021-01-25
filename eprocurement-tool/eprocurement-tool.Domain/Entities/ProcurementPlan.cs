using EGPS.Domain.Common;
using EGPS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Domain.Entities
{
    public class ProcurementPlan : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ProcurementCategoryId { get; set; }
        public ProcurementCategory ProcurementCategory { get; set; }
        public Guid ProcessTypeId { get; set; }
        public ProcurementProcess ProcurementProcess { get; set; }
        public Guid ProcurementMethodId { get; set; }
        public ProcurementMethod ProcurementMethod { get; set; }
        public double EstimatedAmountInNaira { get; set; }
        public double EstimatedAmountInDollars { get; set; }
        public EProcurementStage Stage { get; set; } = EProcurementStage.NOTSTARTED;
        public Guid QualificationMethodId { get; set; }
        public QualificationMethod QualificationMethod { get; set; }
        public Guid ReviewMethodId { get; set; }
        public ReviewMethod ReviewMethod { get; set; }
        public Guid MinistryId { get; set; }
        public Ministry Ministry { get; set; }
        public string Description { get; set; }
        public EProcurementPlanStatus Status { get; set; } = EProcurementPlanStatus.DRAFT;
        public EProcurementSectionStatus SectionOne { get; set; } = EProcurementSectionStatus.INREVIEW;
        public EProcurementSectionStatus SectionTwo { get; set; } = EProcurementSectionStatus.INREVIEW;
        public EProcurementSectionStatus SectionThree { get; set; } = EProcurementSectionStatus.INREVIEW;
        public string PackageNumber { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid GeneralPlanId { get; set; }
        public Guid? CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public GeneralPlan GeneralPlan { get; set; }
        public Contract Contract { get; set; }
        public ICollection<ProcurementPlanActivity> ProcurementPlanActivities { get; set; }
        public ICollection<VendorProcurement> VendorProcurements { get; set; }
    }
}

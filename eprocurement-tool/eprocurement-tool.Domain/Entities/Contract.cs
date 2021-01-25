using System;
using EGPS.Domain.Common;
using EGPS.Domain.Enums;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Domain.Entities
{
    public class Contract : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid? ContractorId { get; set; }
        public string ContractNumber { get; set; }
        public DateTime? SignedDate { get; set; }
        public string EvaluationCurrency { get; set; }
        public Guid UserId { get; set; }
        public double? EstimatedValue { get; set; }
        public ESignatureStatus SignatureStatus { get; set; } = ESignatureStatus.UNSIGNED;
        public Guid? RegistrationPlanId { get; set; }
        public int Duration { get; set; }
        public EDurationType Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double PercentageCompletion { get; set; }
        public EContractStatus Status { get; set; }
        public Guid ProcurementPlanId { get; set; }
        public string ReferenceId { get; set; }

        public bool IsUploaded { get; set; }
        //navigational properties
        public User User { get; set; }
        public User Contractor { get; set; }
        public RegistrationPlan RegistrationPlan { get; set; }
        public ProcurementPlan ProcurementPlan { get; set; }
    }
}

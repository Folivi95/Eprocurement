using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EGPS.Domain.Common;

namespace EGPS.Domain.Entities
{
    public class Datasheet : AuditableEntity
    {
        public Guid Id { get; set; }
        public DateTime? SubmissionDeadline { get; set; }
        public DateTime? SignatureDate { get; set; }
        public DateTime? StartDate { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public Guid ProcurementPlanActivityId { get; set; }
        public ProcurementPlanActivity ProcurementPlanActivity { get; set; }
        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; }
    }
}

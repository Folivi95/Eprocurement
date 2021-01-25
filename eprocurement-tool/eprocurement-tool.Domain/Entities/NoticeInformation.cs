using EGPS.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EGPS.Domain.Entities
{
    public class NoticeInformation : AuditableEntity
    {
        public Guid Id { get; set; }
        public DateTime SubmissionDeadline { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Organization { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public DateTime? RevisedDate { get; set; }
        public DateTime? ActualDate  { get; set; }
        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public Guid ProcurementPlanId { get; set; }
        public ProcurementPlan ProcurementPlan { get; set; }
    }
}

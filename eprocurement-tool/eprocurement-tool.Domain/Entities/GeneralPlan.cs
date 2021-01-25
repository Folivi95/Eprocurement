using EGPS.Domain.Common;
using EGPS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EGPS.Domain.Entities
{
    public class GeneralPlan : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public EGeneralPlanStatus Status { get; set; } = EGeneralPlanStatus.PENDING;
        public string Fax { get; set; }
        public string Website { get; set; }
        public Guid MinistryId { get; set; }
        public Ministry Ministry { get; set; }
        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public ICollection<ProcurementPlan> ProcurementPlans { get; set; }
    }
}

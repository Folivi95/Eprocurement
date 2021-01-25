using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using EGPS.Domain.Common;

namespace EGPS.Domain.Entities
{
    public class Ministry : AuditableEntity
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public Guid CreatedById { get; set; }
        //navigational property
        [ForeignKey("CreatedById")]
        public User CreatedBy { get; set; }
        public string Code { get; set; }
        public Guid EstimatedValueId { get; set; }
        public Guid BidLowerThanId { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public ICollection<ProcurementPlan> ProcurementPlans { get; set; }

        public ICollection<Project> Projects { get; set; }
    }
}

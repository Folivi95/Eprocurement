using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using EGPS.Domain.Common;
using EGPS.Domain.Enums;

namespace EGPS.Domain.Entities
{
    public class ProjectMileStone: AuditableEntity
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public double EstimatedValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid CreatedById { get; set; }
        public bool Deleted { get; set; } = false;
        public User CreatedBy { get; set; }
        public EMilestoneStatus Status { get; set; } = EMilestoneStatus.PENDING;
        public Guid ProjectId { get; set; }
        public Project Project { get; set; } 
        public DateTime? DeletedAt { get; set; }
        //public Guid MilestoneInvoiceId { get; set; }

        //navigational properties
        public ICollection<MilestoneTask> MilestoneTasks { get; set; } = null;
        public MilestoneInvoice MilestoneInvoice { get; set; }
    }
}

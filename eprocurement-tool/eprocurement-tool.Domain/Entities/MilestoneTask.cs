using EGPS.Domain.Common;
using System;
using EGPS.Domain.Enums;

namespace EGPS.Domain.Entities
{
    public class MilestoneTask : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid MileStoneId { get; set;}
        public EMilestoneTaskStatus Status { get; set; } = EMilestoneTaskStatus.PENDING;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double EstimatedValue { get; set; }
        public Guid CreatedById { get; set; }

        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        //navigational property
        public User CreatedBy { get; set; }
        public ProjectMileStone MileStone { get; set; }
    }
}

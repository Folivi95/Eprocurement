using EGPS.Application.Common;
using EGPS.Domain.Enums;
using System;

namespace EGPS.Application.Models
{
    public class MilestoneTaskDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public double EstimatedValue { get; set; }
        public string CreatedBy { get; set; }
    }

    public class MilestoneTaskForCreateDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double EstimatedValue { get; set; }
    }
}

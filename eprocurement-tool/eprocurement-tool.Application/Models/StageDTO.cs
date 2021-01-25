using EGPS.Application.Common;
using EGPS.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;

namespace EGPS.Application.Models
{
    public class StageDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid WorkFlowId { get; set; }
        public string Title { get; set; }
        public int Index { get; set; }
        public string GroupClass { get; set; }
        public string Action { get; set; }
        public string UserType { get; set; }
        public int MinimumPass { get; set; }
        public Guid CreatedById { get; set; }
        public bool Deleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public string[] AssigneeIds { get; set; }
        public string[] GroupIds { get; set; }
    }

    public class StageForGetDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid WorkFlowId { get; set; }
        public string Title { get; set; }
        public int Index { get; set; }
        public string GroupClass { get; set; }
        public string Action { get; set; }
        public string UserType { get; set; }
        public int MinimumPass { get; set; }
        public Guid CreatedById { get; set; }
        public bool Deleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public string[] AssigneeIds { get; set; }
        public string[] GroupIds { get; set; }
        public DepartmentDTO Department { get; set; }
    }
}

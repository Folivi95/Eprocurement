using EGPS.Domain.Common;
using EGPS.Domain.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EGPS.Domain.Entities
{
    public class Stage : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Index { get; set; }
        public EGroupClass? GroupClass { get; set; }
        public EAction Action { get; set; }
        [Required]
        public EUserType UserType { get; set; }
        public int MinimumPass { get; set; }
        public bool Deleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public Workflow Workflow { get; set; }
        public Guid WorkFlowId { get; set; }
        public Guid AccountId { get; set; }
        public string AssigneeIds { get; set; }
        public Guid CreatedById { get; set; }
        public string GroupIds { get; set; }
    }
}

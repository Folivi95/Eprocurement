using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EGPS.Domain.Common;

namespace EGPS.Domain.Entities
{
    public class Unit: AuditableEntity
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public Guid LeadId { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid CreatedById { get; set; }
        public bool Deleted { get; set; } = false;
        public ICollection<User> Users { get; set; }
        public ICollection<UnitMember> UnitMembers { get; set; }
    }
}

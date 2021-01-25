using EGPS.Domain.Common;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EGPS.Domain.Entities
{
    public class Role : AuditableEntity
    {
        public Role()
        {
            Resources = new Collection<RoleResource>();
        }
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid? AccountId { get; set; }
        public Guid? CreatedById { get; set; }
        public bool Deleted { get; set; } = false;
        public string Type { get; set; } = "CUSTOM";
        public ICollection<RoleResource> Resources { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}

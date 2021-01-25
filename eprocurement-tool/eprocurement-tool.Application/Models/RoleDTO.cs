using EGPS.Application.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EGPS.Application.Models
{
    public class RoleDTO: AuditableModelDTO
    {
        public RoleDTO()
        {
            Resources = new Collection<RoleResourceDTO>();
        }
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public ICollection<RoleResourceDTO> Resources { get; set; }
        public Guid CreatedById { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class RoleResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}

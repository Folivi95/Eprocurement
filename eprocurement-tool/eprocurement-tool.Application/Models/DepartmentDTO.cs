using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using EGPS.Application.Common;
using EGPS.Domain.Entities;
using Newtonsoft.Json;

namespace EGPS.Application.Models
{
    public class DepartmentDTO: AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid LeadId { get; set; }
        public string Description { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid CreatedById { get; set; }
        public Guid AccountId { get; set; }
        public bool Deleted { get; set; }
        public string Website { get; set; }
    }

    public class DepartmentsDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid LeadId { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid CreatedById { get; set; }
        public Guid AccountId { get; set; }
        public bool Deleted { get; set; }
        public int TotalMembers { get; set; }
        public ICollection<DepartmentUserDTO> Members { get; set; }
    }

    public class DepartmentUserDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName  { get; set; }
        public object ProfilePicture { get; set; }
    }
}

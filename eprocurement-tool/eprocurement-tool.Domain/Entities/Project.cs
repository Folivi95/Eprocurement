using EGPS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EGPS.Domain.Common;

namespace EGPS.Domain.Entities
{
    public class Project : AuditableEntity
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public EProjectStatus Status { get; set; } = EProjectStatus.INACTIVE;
        public double? EstimatedValue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid CreatedById { get; set; }
        public Guid ContractId { get; set; }
        public Contract Contract { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid MinistryId { get; set; }

        public Guid VendorId { get; set; }

        //navigational property
        public User CreatedBy { get; set; }

     
        public ICollection<ProjectMileStone> ProjectMileStones { get; set; }
        public Ministry Ministry { get; set; }
    }
}

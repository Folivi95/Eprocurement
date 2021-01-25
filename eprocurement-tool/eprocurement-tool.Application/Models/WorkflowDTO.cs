using EGPS.Application.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EGPS.Application.Models
{
    public class WorkflowDTO : AuditableModelDTO
    {
        public WorkflowDTO()
        {
            Stages = new Collection<StageDTO>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid AccountId { get; set; }
        public Guid CreatedById { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Deleted { get; set; }
        public ICollection<StageDTO> Stages { get; set; }
    }

    public class WorkflowForGetDTO : AuditableModelDTO
    {
        public WorkflowForGetDTO()
        {
            Stages = new Collection<StageForGetDTO>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid AccountId { get; set; }
        public Guid CreatedById { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Deleted { get; set; }
        public ICollection<StageForGetDTO> Stages { get; set; }
    }

    public class WorkflowsDTO : AuditableModelDTO
    {
        public WorkflowsDTO()
        {
            Stages = new Collection<StageDTO>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid AccountId { get; set; }
        public Guid CreatedById { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool Deleted { get; set; }
        public int TotalStages { get; set; }
        public ICollection<StageDTO> Stages { get; set; }
    }
}

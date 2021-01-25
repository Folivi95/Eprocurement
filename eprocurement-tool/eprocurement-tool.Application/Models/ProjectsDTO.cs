using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using AutoMapper.Configuration.Annotations;
using EGPS.Application.Common;
using EGPS.Domain.Enums;
using EGPS.Domain.Common;
using EGPS.Domain.Entities;

namespace EGPS.Application.Models
{
    public class ProjectsDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } 
        public double EstimatedValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid CreatedById { get; set; }
        public Guid ContractId { get; set; }
        public RegistrationPlanDTO RegistrationPlan { get; set; }
        public Guid VendorId { get; set; }

        [Ignore]
        public decimal PercentageCompleted { get; set; }
        public MinistryDTO Ministry { get; set; } 

        //navigational property
        public ICollection<ProjectMileStoneDTO> ProjectMileStones { get; set; }
    }

    public class ProjectDetailsDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public EProjectStatus Status { get; set; }
        public double EstimatedValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid CreatedById { get; set; }
        public Contract Contract { get; set; }
    }



    public class ProjectsSummaryDTO
    {
        public int Total { get; set; }
        public int Active { get; set; }
        public int Completed { get; set; }
        public int Inactive { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper.Configuration.Annotations;
using EGPS.Application.Common;
using EGPS.Domain.Common;
using EGPS.Domain.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EGPS.Application.Models
{
    
    public class ProjectMileStoneDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double EstimatedValue { get; set; }
        public Guid ProjectId { get; set; } 
        public ProjectsDTO Project { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid CreatedById { get; set; } 

        [Ignore]
        public decimal PercentageCompleted { get; set; }
        //public EMilestoneStatus Status { get; set; } = EMilestoneStatus.PENDING;

        public string Status { get; set; } = "PENDING";
        public MilestoneInvoiceDTO MilestoneInvoice { get; set; }

        public IEnumerable<MilestoneTaskDTO> MilestoneTasks { get; set; }
    }

    public class MilestoneInvoiceDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime? DueDate { get; set; }
        public string InvoiceNumber { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EPaymentStatus PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? DeclinedDate { get; set; }
        public EInvoiceStatus Status { get; set; } = EInvoiceStatus.PAID;
        public ProjectMileStoneDTO ProjectMileStone { get; set; } 
        public string UniqueId { get; set; } 
        [Ignore]
        public ProjectsDTO Project { get; set; }


    }
}

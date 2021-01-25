using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Common;
using EGPS.Domain.Common;
using EGPS.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace EGPS.Application.Models
{
    
    public class ProjectMileStoneForCreationDTO: AuditableEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double? EstimatedValue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid CreatedById { get; set; }
    }

    public class UpdateProjectMileStone
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public double? EstimatedValue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }

    public class MilestoneInvoiceForCreation
    {
        public DateTime DueDate { get; set; }
        // public List<IFormFile> Documents { get; set; } = new List<IFormFile>();
        // public EDocumentStatus DocumentStatus { get; set; }
    }

}

using EGPS.Application.Common;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class VendorProcurementDTO : AuditableModelDTO
    {
        public string Reason { get; set; }
        public string Currency { get; set; }
        public decimal? Amount { get; set; }
        public decimal? BidPrice { get; set; }
        public decimal? EvaluatedPrice { get; set; }
        public EVendorContractStatus? Type { get; set; }
        public int Position { get; set; }
        public Guid ProcurementPlanId { get; set; }
        public ProcurementPlanDTO ProcurementPlan { get; set; }
        public Guid VendorId { get; set; }
    }

    public class VendorContractUploadDTO 
    {
        public Guid ProcurementPlanId { get; set; }
        public Guid ProcurementPlanActivityId { get; set; }
        public List<IFormFile> Documents { get; set; }
    }
}

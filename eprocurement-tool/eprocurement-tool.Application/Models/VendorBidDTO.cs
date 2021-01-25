using EGPS.Application.Common;
using EGPS.Domain.Enums;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace EGPS.Application.Models
{
    public class VendorBidDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public Guid VendorId { get; set; }
        public Guid ProcurementPlanId { get; set; }
        public string Reason { get; set; }
        public string Currency { get; set; }
        public decimal? Amount { get; set; }
        public int Position { get; set; }
        public decimal? BidPrice { get; set; }
        public decimal? EvaluatedPrice { get; set; }
        public string Type { get; set; }
        public EvaluatedBidResponse VendorProfile { get; set; }

        //navigational properties
        public ProcurementPlanDTO ProcurementPlan { get; set; }
    }

    public class VendorBidForProcurementPlanDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public Guid VendorId { get; set; }
        public VendorProfileDTO VendorProfile { get; set; }
        public Guid ProcurementPlanId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Common;
using EGPS.Domain.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EGPS.Application.Models
{
    public class GeneralPlanDTO: AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EGeneralPlanStatus Status { get; set; } 
        public string Fax { get; set; }
        public string Website { get; set; }
        public Guid MinistryId { get; set; }
        public MinistryPlan Ministry { get; set; }
        public Guid CreatedById { get; set; }
    }

    public class GeneralPlanResponse : GeneralPlanDTO
    {
        public int Categories { get; set; }
        public double Amount { get; set; }
    }

    public class GeneralPlanForCreation
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public Guid MinistryId { get; set; }
    }

    public class MinistryPlan
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class GeneralPlanSummaryDto
    {
        public int Total { get; set; }
        public int ApprovedTotal  { get; set; }
        public int PendingTotal { get; set; }
    }

    public class GeneralPlanForUpdate
    {
        public string Description { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public Guid MinistryId { get; set; }
    }

    public class ProcurementPlanSummaryDto
    {
        public WorkCategory Works { get; set; }
        public GoodCategory Goods { get; set; }
        public ConsultancyCategory Consultancy { get; set; }
        public NonConsultancyCategory NonConsultancy { get; set; }
    }

    public class WorkCategory
    {
        public int Total { get; set; }
        public int Approved { get; set; }
        public int Incomplete { get; set; }
    }

    public class GoodCategory
    {
        public int Total { get; set; }
        public int Approved { get; set; }
        public int Incomplete { get; set; }
    }

    public class ConsultancyCategory
    {
        public int Total { get; set; }
        public int Approved { get; set; }
        public int Incomplete { get; set; }
    }

    public class NonConsultancyCategory
    {
        public int Total { get; set; }
        public int Approved { get; set; }
        public int Incomplete { get; set; }
    }

    public class ProcurementGroup
    {
        public string Category { get; set; }
        public EProcurementPlanStatus Status { get; set; }
        public int Count { get; set; }
    }
}

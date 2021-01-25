using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Common;
using EGPS.Domain.Common;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EGPS.Application.Models
{
    public class ProcurementPlanToReturnDto : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid GeneralPlanId { get; set; }
        public ProcurementCategory ProcurementCategory { get; set; }
        public ProcurementProcess ProcurementProcess { get; set; }
        public ProcurementMethod ProcurementMethod { get; set; }
        public double EstimatedAmountInNaira { get; set; }
        public double EstimatedAmountInDollars { get; set; }
        public QualificationMethod QualificationMethod { get; set; }
        public ReviewMethod ReviewMethod { get; set; }
        public string Ministry { get; set; }
        public string MinistryCode { get; set; }
        public string Description { get; set; }
        public Guid CreatedById { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EProcurementPlanStatus Status { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EProcurementSectionStatus SectionOne { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EProcurementSectionStatus SectionTwo { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EProcurementSectionStatus SectionThree { get; set; }
        public string PackageNumber { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public IEnumerable<ProcurementActivityDTO> ProcurementPlanActivities { get; set; }
    }
}

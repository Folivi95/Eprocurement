using EGPS.Application.Common;
using EGPS.Domain.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class RegistrationPlanDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Grade { get; set; }
        public decimal Fee { get; set; }
        public decimal ContractMinValue { get; set; }
        public decimal ContractMaxValue { get; set; }
        public int TenureInDays { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ERegistrationCategoryType RegistrationCategoryType { get; set; }
        public Guid CreatedBy { get; set; }
    }
}

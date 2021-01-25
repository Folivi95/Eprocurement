using EGPS.Domain.Common;
using EGPS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Domain.Entities
{
    public class RegistrationPlan : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Grade { get; set; }
        public decimal Fee { get; set; }
        public decimal ContractMinValue { get; set; }
        public decimal ContractMaxValue { get; set; }
        public ERegistrationCategoryType RegistrationCategoryType { get; set; }
        public int TenureInDays { get; set; }
        public Guid CreatedBy { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}

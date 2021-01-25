using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Domain.Common;

namespace EGPS.Domain.Entities
{
    public class ProcurementPlanNumber : AuditableEntity
    {
        public Guid Id { get; set; }
        public string StateCode { get; set; }
        public string MinistryCode { get; set; }
        public string ProcurementCategoryCode { get; set; }
        public string ProcurementMethodCode { get; set; }
        public int SerialNumber { get; set; }
        public int Year { get; set; }

        public string PlanNumber { get; set; }
    }
}

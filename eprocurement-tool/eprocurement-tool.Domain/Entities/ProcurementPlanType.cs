using EGPS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Domain.Entities
{
    public class ProcurementPlanType
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public EPprocurementPlanTask ProcurementPlanTask { get; set; }
    }
}

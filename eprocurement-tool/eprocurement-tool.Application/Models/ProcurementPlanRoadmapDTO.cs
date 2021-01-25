using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Common;
using EGPS.Domain.Enums;

namespace EGPS.Application.Models
{
    public class ProcurementPlanRoadmapDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public EPprocurementPlanTask PprocurementPlanTask { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}

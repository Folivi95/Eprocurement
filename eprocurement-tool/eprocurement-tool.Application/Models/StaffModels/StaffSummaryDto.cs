using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Common;

namespace EGPS.Application.Models.StaffModels
{
    public class StaffSummaryDto : AuditableModelDTO
    {
        public int Total { get; set; }
        public int ActiveTotal { get; set; }
        public int PendingTotal { get; set; }
    }
}

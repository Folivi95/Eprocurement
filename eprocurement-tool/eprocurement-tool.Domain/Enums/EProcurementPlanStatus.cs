using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EGPS.Domain.Enums
{
    public enum EProcurementPlanStatus
    {
        [Description("APPROVED")]
        APPROVED = 1,
        [Description("INREVIEW")] 
        INREVIEW = 2,
        [Description("DRAFT")]
        DRAFT = 3
    }
}

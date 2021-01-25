using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EGPS.Domain.Enums
{
    public enum EProcurementPlanActivityStatus
    {
        [Description("Approved")]
        APPROVED = 1,

        [Description("Pending")]
        PENDING = 2,

        [Description("InActive")]
        INACTIVE = 3
    }
}

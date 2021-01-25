using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EGPS.Domain.Enums
{
    public enum EPprocurementPlanTask
    {
        [Description("PROCUREMENTPLANNING")]
        PROCUREMENTPLANNING = 1,

        [Description("PROCUREMENTEXECUTION")]
        PROCUREMENTEXECUTION = 2
    }
}

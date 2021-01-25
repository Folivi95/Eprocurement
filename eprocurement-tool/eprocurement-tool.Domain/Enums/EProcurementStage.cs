using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EGPS.Domain.Enums
{
    public enum EProcurementStage
    {
        [Description("ONGOING")]
        ONGOING = 1,

        [Description("COMPLETED")]
        COMPLETED = 2,

        [Description("NOTSTARTED")]
        NOTSTARTED = 3
    }
}

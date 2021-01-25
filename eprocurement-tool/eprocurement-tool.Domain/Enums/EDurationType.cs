using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EGPS.Domain.Enums
{
    public enum EDurationType
    {
        [Description("YEAR")]
        YEAR = 1,

        [Description("MONTH")]
        MONTH = 2,

        [Description("WEEK")]
        WEEK = 3,

        [Description("DAY")]
        DAY = 4,
    }
}

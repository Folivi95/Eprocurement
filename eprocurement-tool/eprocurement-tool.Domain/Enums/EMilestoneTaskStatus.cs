using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EGPS.Domain.Enums
{
    public enum EMilestoneTaskStatus
    {
        [Description("PENDING")] PENDING = 1,

        [Description("INPROGRESS")] INPROGRESS = 2,

        [Description("DONE")] DONE = 3,
    }
}

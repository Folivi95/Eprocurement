using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EGPS.Domain.Enums
{
    public enum EProjectStatus
    {
        [Description("ACTIVE")] 
        ACTIVE = 1,

        [Description("INACTIVE")] 
        INACTIVE = 2,

        [Description("COMPLETED")]
        COMPLETED = 3
    }
}

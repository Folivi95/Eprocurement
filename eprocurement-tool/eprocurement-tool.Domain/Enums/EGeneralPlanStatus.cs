using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EGPS.Domain.Enums
{
    public enum EGeneralPlanStatus
    {
        [Description("APPROVED")]
        APPROVED = 1,
        [Description("PENDING")]
        PENDING = 2,
        [Description("NEED AMENDMENT")] 
        NEEDAMENDMENT = 3
    }
}

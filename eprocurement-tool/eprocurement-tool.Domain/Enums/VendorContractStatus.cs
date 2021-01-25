using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EGPS.Domain.Enums
{
    public enum EVendorContractStatus
    {
        [Description("Recommended")]
        RECOMMENDED = 1,

        [Description("Evaluated")]
        EVALUATED = 2,

        [Description("Rejected")]
        REJECTED = 3,

        [Description("Processing")]
        PROCESSING = 4,

        [Description("NotStarted")]
        NOTSTARTED = 5,

        [Description("Expired")]
        EXPIRED = 6,

        [Description("Interested")]
        INTERESTED = 7
    }
}

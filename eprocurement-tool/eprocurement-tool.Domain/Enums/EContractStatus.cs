using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EGPS.Domain.Enums
{
    public enum EContractStatus
    {
        [Description("ACCEPTED")]
        ACCEPTED = 1,

        [Description("REJECTED")]
        REJECTED = 2,

        [Description("PENDING")]
        PENDING = 3,
        [Description("EXPIRED")]
        EXPIRED = 4,

        [Description("CLOSED")]
        CLOSED = 5

    }
}

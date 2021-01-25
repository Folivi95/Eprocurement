using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EGPS.Domain.Enums
{
    public enum EPaymentStatus
    {
        [Description("PAID")]
        PAID = 1,

        [Description("PENDING")]
        PENDING = 2,
    }
}

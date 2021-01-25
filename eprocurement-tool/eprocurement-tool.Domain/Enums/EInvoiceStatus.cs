using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EGPS.Domain.Enums
{
    public enum EInvoiceStatus
    {
        [Description("PAID")]
        PAID = 1,

        [Description("PENDING")]
        PENDING = 2,

        [Description("REJECTED")]
        REJECTED = 3,
    }
}

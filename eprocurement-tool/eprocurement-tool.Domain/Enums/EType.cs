using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EGPS.Domain.Enums
{
    public enum EType
    {
        [Description("Email")]
        Email = 1,

        [Description("SMS")]
        SMS = 2,

        [Description("Push")]
        Push = 3,

        [Description("InApp")]
        InApp = 4
    }
}

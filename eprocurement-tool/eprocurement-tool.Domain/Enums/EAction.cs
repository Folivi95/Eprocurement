using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EGPS.Domain.Enums
{
    public enum EAction
    {
        [Description("CHECK")]
        CHECK = 1,

        [Description("APPROVAL")]
        APPROVAL = 2
    }
}

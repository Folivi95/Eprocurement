using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EGPS.Domain.Enums
{
    public enum ESignatureStatus
    {
        [Description("SIGNED")]
        SIGNED = 1,

        [Description("UNSIGNED")]
        UNSIGNED = 2,
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EGPS.Domain.Enums
{
    public enum EDocumentStatus
    {
        [Description("MANDATORY")]
        MANDATORY = 1,

        [Description("SUPPORTING")]
        SUPPORTING = 2,

        [Description("REVIEW")]
        REVIEW = 3,

        [Description("PAYMENT")]
        PAYMENT = 4,
        
        [Description("OTHER")]
        OTHER = 5
    }
}

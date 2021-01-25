using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EGPS.Domain.Enums
{
    public enum EProcurementSectionStatus
    {
        [Description("INREVIEW")] 
        INREVIEW = 1,

        [Description("NEEDAMENDMENT")] 
        NEEDAMENDMENT = 2,


        [Description("APPROVED")] 
        APPROVED = 3
    }
}

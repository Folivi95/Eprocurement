using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EGPS.Domain.Enums
{
    public enum CommentType
    {
        [Description("SUGGESTION")] 
        SUGGESTION = 1,

        [Description("COMMENT")] 
        COMMENT = 2,

        [Description("COMPLAINT")]
        COMPLAINT = 3
    }

    public enum ObjectClass
    {
        [Description("TENDER")] 
        TENDER = 1,

        [Description("BID")] 
        BID = 2
    }
}

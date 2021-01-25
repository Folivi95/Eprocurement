using System.ComponentModel;

namespace EGPS.Domain.Enums
{
    public enum EVendorDocumentStatus
    {
        [Description("APPROVED")]
        APPROVED = 1,
        
        [Description("PENDING")]
        PENDING = 2,

        [Description("REJECTED")]
        REJECTED = 3
    }
}

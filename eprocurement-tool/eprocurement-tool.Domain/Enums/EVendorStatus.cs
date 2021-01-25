using System.ComponentModel;

namespace EGPS.Domain.Enums
{
    public enum EVendorStatus
    {
        [Description("PENDING")]
        PENDING = 1,

        [Description("APPROVED")]
        APPROVED = 2,

        [Description("REJECTED")]
        REJECTED = 3,

        [Description("BLACKLISTED")]
        BLACKLISTED = 4,
    }
}

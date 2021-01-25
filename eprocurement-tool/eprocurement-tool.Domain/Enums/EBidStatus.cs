using System.ComponentModel;

namespace EGPS.Domain.Enums
{
    public enum EBidStatus
    {
        [Description("ACTIVE")]
        ACTIVE = 1,

        [Description("CLOSED")]
        CLOSED = 2,

        [Description("PROCESSING")]
        PROCESSING = 3,

        [Description("NOTSTARTED")]
        NOTSTARTED = 4,
    }
}

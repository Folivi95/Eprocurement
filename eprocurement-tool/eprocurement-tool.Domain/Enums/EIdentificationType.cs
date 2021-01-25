using System.ComponentModel;

namespace EGPS.Domain.Enums
{
    public enum EIdentificationType
    {
        [Description("NATIONAL_IDENTITY_CARD")]
        NATIONAL_IDENTITY_CARD = 1,

        [Description("PERMANENT_VOTER_CARD")]
        PERMANENT_VOTER_CARD = 2,

        [Description("TEMPORTATY_VOTER_CARD")]
        TEMPORTATY_VOTER_CARD = 3,

        [Description("DRIVER_LICENCE")]
        DRIVER_LICENCE = 4,

        [Description("INTERNATIONAL_PASSPORT")]
        INTERNATIONAL_PASSPORT = 5,

        [Description("NATIONAL_IDENTITY_NUMBER_SLIP")]
        NATIONAL_IDENTITY_NUMBER_SLIP = 6
    }
}

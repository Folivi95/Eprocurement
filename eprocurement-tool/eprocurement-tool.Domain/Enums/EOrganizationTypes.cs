using System.ComponentModel;

namespace EGPS.Domain.Enums
{
    public enum EOrganizationTypes
    {
        [Description("INCORPORATED_COMPANY")]
        INCORPORATED_COMPANY = 1,

        [Description("LIMITED_PARTNERSHIPS")]
        LIMITED_PARTNERSHIPS = 2,

        [Description("INDIVIDUAL_CONSULTANTS")]
        INDIVIDUAL_CONSULTANTS = 3,
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EGPS.Domain.Enums
{
    public enum ENotificationType
    {
        [Description("Email")]
        Email = 1,

        [Description("Feed")]
        Feed = 2
    }
}

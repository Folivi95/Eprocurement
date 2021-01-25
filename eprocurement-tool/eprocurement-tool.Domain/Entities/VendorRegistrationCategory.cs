using EGPS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Domain.Entities
{
    public class VendorRegistrationCategory : AuditableEntity
    {
        public Guid UserId { get; set; }
        public Guid RegistrationPlanId { get; set; }
        public User User { get; set; }
        public RegistrationPlan RegistrationPlan { get; set; }
    }
}

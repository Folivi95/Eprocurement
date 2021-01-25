using EGPS.Domain.Common;
using System;
using System.Collections.Generic;

namespace EGPS.Domain.Entities
{
    public class VendorService : AuditableEntity
    {
        public Guid UserID { get; set; }
        public Guid BusinessServiceID { get; set; }

        public User User { get; set; }

        public BusinessService BusinessServices { get; set; }

    }
}

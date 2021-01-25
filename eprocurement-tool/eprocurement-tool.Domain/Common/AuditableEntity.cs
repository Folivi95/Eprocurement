using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Domain.Common
{
    public class AuditableEntity
    {
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}

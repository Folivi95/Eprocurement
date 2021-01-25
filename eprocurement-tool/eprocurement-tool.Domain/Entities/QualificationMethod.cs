using EGPS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Domain.Entities
{
    public class QualificationMethod : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string  Description { get; set; }
    }
}

using EGPS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Domain.Entities
{
    public class VendorDirectorCertificate : AuditableEntity
    {
        public Guid Id { get; set; }
        public Guid VendorDirectorId { get; set; }
        public Guid CreatedById { get; set; }
        public string File { get; set; }
        public bool Deleted { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}

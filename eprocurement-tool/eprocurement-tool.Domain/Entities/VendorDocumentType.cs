using System;
using EGPS.Domain.Common;

namespace EGPS.Domain.Entities
{
    public class VendorDocumentType : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid CreatedById { get; set; }
        public bool Deleted { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}

using System;
using EGPS.Domain.Common;
using EGPS.Domain.Enums;

namespace EGPS.Domain.Entities
{ 
    public class VendorDocument : AuditableEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name  { get; set; }
        public string Description { get; set; }
        public string File { get; set; }
        public Guid VendorDocumentTypeId { get; set; }
        public EVendorDocumentStatus Status  { get; set; }
        public Guid CreatedById { get; set; }
        public bool Deleted { get; set; }
        public DateTime DeletedAt { get; set; }

        //navigation property
        public VendorDocumentType VendorDocumentType { get; set; }
    }
}

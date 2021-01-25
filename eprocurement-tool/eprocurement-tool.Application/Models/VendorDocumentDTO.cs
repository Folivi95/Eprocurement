using System;
using EGPS.Application.Common;
using EGPS.Domain.Enums;

namespace EGPS.Application.Models
{
    public class VendorDocumentDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Name  { get; set; }
        public string Description { get; set; }
        public string File { get; set; }
        public Guid VendorDocumentTypeId { get; set; }
        public EVendorDocumentStatus Status  { get; set; }
        public Guid CreatedById { get; set; }
    }
}

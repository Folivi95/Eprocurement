using System;
using EGPS.Application.Common;

namespace EGPS.Application.Models
{
    public class VendorDocumentTypeDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid CreatedById { get; set; }
    }
}

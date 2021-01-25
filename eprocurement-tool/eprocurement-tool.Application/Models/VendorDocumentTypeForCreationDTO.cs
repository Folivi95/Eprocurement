using System;

namespace EGPS.Application.Models
{
    public class VendorDocumentTypeForCreationDTO
    {
        public string Title { get; set; }
        public Guid CreatedById { get; set; }
        public bool Deleted { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}

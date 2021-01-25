using System;
using EGPS.Application.Common;

namespace EGPS.Application.Models
{
    public class VendorAttestationDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime AttestedAt { get; set; }
        public Guid CreatedById { get; set; }
    }
}

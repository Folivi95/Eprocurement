using System;
using EGPS.Domain.Common;

namespace EGPS.Domain.Entities
{
    public class VendorAttestation : AuditableEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime AttestedAt { get; set; }
        public Guid CreatedById { get; set; }
    }
}

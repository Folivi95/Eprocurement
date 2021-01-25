using System;
using EGPS.Domain.Common;

namespace EGPS.Domain.Entities
{
    public class VendorContact : AuditableEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
    }
}

using System;

namespace EGPS.Application.Models
{
    public class VendorAttestationForCreationDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime AttestedAt { get; set; }
        public Guid CreatedById { get; set; }
    }
}

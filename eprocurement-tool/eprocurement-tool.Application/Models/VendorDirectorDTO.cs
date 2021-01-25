using System;
using EGPS.Application.Common;
using EGPS.Domain.Enums;

namespace EGPS.Application.Models
{
    public class VendorDirectorDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }	 
        public string FirstName { get; set; }
        public string Title { get; set; }
        public string LastName { get; set; }	
        public string PhoneNumber { get; set; }
        public EIdentificationType IdentificationType { get; set; }
        public string AddressLine1 { get; set; }	        
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }	
        public string Country { get; set; }	
        public string PassportPhoto { get; set; }
        public string IdentificationFile { get; set; }
        public VendorDirectorCertificateDTO[] Certifications { get; set; }
        public Guid UserId { get; set; }
        public Guid CreatedById { get; set; }
        public bool Deleted { get; set; }
        public DateTime DeletedAt { get; set; }
    }
}

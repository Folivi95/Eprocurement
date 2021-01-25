using System;
using System.Collections.Generic;
using EGPS.Domain.Common;
using EGPS.Domain.Enums;

namespace EGPS.Domain.Entities
{
    public class VendorDirector : AuditableEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }	 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }	   
        public string AddressLine1 { get; set; }	        
        public string AddressLine2 { get; set; }	
        public string City { get; set; }	
        public string State { get; set; }	
        public string Country { get; set; }	
        public string PassportPhoto { get; set; }
        public string IdentificationFile { get; set; }
        public Guid UserId { get; set; }
        public EIdentificationType IdentificationType { get; set; }
        public Guid CreatedById { get; set; }
        public bool Deleted { get; set; }
        public DateTime DeletedAt { get; set; }

        //navigation properties
        public ICollection<VendorDirectorCertificate> Certifications { get; set; }
    }
}

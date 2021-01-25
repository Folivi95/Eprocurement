using System;
using System.Collections.Generic;
using EGPS.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace EGPS.Application.Models
{
    public class VendorDirectorForCreationDTO
    {
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
        public IFormFile PassportPhoto { get; set; }
        public EIdentificationType IdentificationType { get; set; }
        public IFormFile IdentificationFile { get; set; } 
        public List<IFormFile> Certifications { get; set; } = new List<IFormFile>();
    }


    public class VendorDirectorForUpdateDTO
    {
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
        public IFormFile PassportPhoto { get; set; }
        public EIdentificationType IdentificationType { get; set; }
        public IFormFile IdentificationFile { get; set; }
        public List<IFormFile> Certifications { get; set; } = new List<IFormFile>();
    }
}

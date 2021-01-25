using System;
using System.ComponentModel.DataAnnotations;
using EGPS.Application.Common;
using EGPS.Domain.Enums;

namespace EGPS.Application.Models
{
    public class VendorProfileForCreationDTO 
    {
        public string CompanyName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        [DataType(DataType.Url)]
        public string Website { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public ECoreCompetencies CoreCompetency { get; set; }
        public EOrganizationTypes OrganizationType { get; set; }
        public DateTime IncorporationDate { get; set; }
        public string CACRegistrationNumber { get; set; }
        public string AuthorizedShareCapital { get; set; }


        public string CorrespondenceAddress1 { get; set; }
        public string CorrespondenceAddress2 { get; set; }
        public string CorrespondenceCity { get; set; }
        public string CorrespondenceState { get; set; }
        public string CorrespondenceCountry { get; set; }


        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPosition { get; set; }

    }

    public class VendorProfileUserForCreationDTO
    {
       
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }


    public class VendorProfileForUpdateDTO
    {
        public ECoreCompetencies CoreCompetency { get; set; }
        public EOrganizationTypes OrganizationType { get; set; }
        public string CompanyName { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public DateTime IncorporationDate { get; set; }
        public string CACRegistrationNumber { get; set; }
        public string AuthorizedShareCapital { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public VendorCorrespondenceForCreationDTO Correspondences { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public EVendorStatus Status { get; set; }

        [DataType(DataType.Url)]
        public string Website { get; set; }
        public string JobTitle { get; set; }
    }

    public class VendorProfileRegistrationPlansForUpdateDTO
    {
        public Guid registrationPlanId { get; set; }
    }

    public class UpdateVendorRegPayStatusCreationDTO
    {
        public string PaymentResponseCode { get; set; }
        public string TransactionId { get; set; }
    }
}

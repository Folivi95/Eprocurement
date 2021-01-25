using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EGPS.Application.Common;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EGPS.Application.Models
{
    public class VendorProfileDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        [DataType(DataType.Url)]
        public string Website { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Status { get; set; }
        public ECoreCompetencies CoreCompetency { get; set; }
        public EOrganizationTypes OrganizationType { get; set; }
        public VendorCorrespondenceDTO VendorCorrespondence { get; set; }
        public VendorContactDTO VendorContact { get; set; }
        public DateTime IncorporationDate { get; set; }
        public string CACRegistrationNumber { get; set; }
        public string AuthorizedShareCapital { get; set; }
        public Guid RegistrationPlanId { get; set; }
        public bool IsRegistrationComplete { get; set; }
        public RegistrationPlanDTO RegistrationPlan { get; set; }
    }


    public class VendorProfileUserDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool EmailVerified { get; set; }
        public EUserType UserType { get; set; }
        public VendorProfileDTO Profile { get; set; }
        public string JobTitle { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public EStatus Status { get; set; }
        public DateTime? LastLogin { get; set; }
        public Guid CreatedById { get; set; }
    }

    public class UserVendorDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string HomeAddress { get; set; }
        public bool EmailVerified { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EStatus Status { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EUserType UserType { get; set; }
        public object ProfilePicture { get; set; }
        public DateTime? LastLogin { get; set; }
        public Guid MinistryId { get; set; }
        public Guid UserRoleId { get; set; }
        public MinistryDTO Ministry { get; set; }
        public ICollection<UserRoleDTO> UserRoles { get; set; }
        public VendorProfileDTO VendorProfile { get; set; }
    }

    public class VendorSummaryDto
    {
        public int Total { get; set; }
        public int ActiveTotal { get; set; }
        public int PendingTotal { get; set; }
        public int RejectedTotal { get; set; }
        public int BlacklistedTotal { get; set; }
    }

    public class BidEvaluationVendorDto : AuditableModelDTO
    {
        public Guid UserId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Website { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public EVendorStatus Status { get; set; }
        public ECoreCompetencies CoreCompetency { get; set; }
        public EOrganizationTypes OrganizationType { get; set; }
        public DateTime IncorporationDate { get; set; }
        public string CACRegistrationNumber { get; set; }
        public string AuthorizedShareCapital { get; set; }
        public Guid RegistrationPlanId { get; set; }
        public bool IsRegistrationComplete { get; set; }
        public RegistrationPlanDTO RegistrationPlan { get; set; }
    }

    public class InitiatePaymentDTO
    {
        public Guid VendorProfileId { get; set; }
        public string PublicKey { get; set; }
        public string TransactionId { get; set; }
        public string CallbackUrl { get; set; }
        public string Hash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Amount { get; set; }
        public string Currency { get; } = "NGN";
        public string Country { get; } = "NG";
        public string LogoUrl { get; set; } = "";
    }

    public class UpdatePaymentDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; } = "NGN";

        public string TransactionId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EPaymentStatus PaymentStatus { get; set; }
    }

    public class VerifyPaymentDTO
    {
        public string status { get; set; }
        public VerifyPaymentData data { get; set; }
    }

    public class VerifyPaymentData
    {
        public PaymentData payment { get; set; }
    }

    public class PaymentData : AuditableModelDTO
    {
        public string paymentResponseCode { get; set; }
        public string paymentResponseMessage { get; set; }
        public string transactionId { get; set; }
        public string amount { get; set; }
    }

    public class EvaluatedBidResponse : AuditableModelDTO
    {
        public Guid UserId { get; set; }
        public string CompanyName { get; set; }
    }
}

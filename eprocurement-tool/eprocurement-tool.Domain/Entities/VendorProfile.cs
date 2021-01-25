using System;
using System.Collections.Generic;
using EGPS.Domain.Common;
using EGPS.Domain.Enums;

namespace EGPS.Domain.Entities
{
    public class VendorProfile : AuditableEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public EVendorStatus Status { get; set; }
        public string CompanyName { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Website { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public ECoreCompetencies CoreCompetency { get; set; }
        public EOrganizationTypes OrganizationType { get; set; }
        public DateTime IncorporationDate { get; set; }
        public string CACRegistrationNumber { get; set; }
        public string AuthorizedShareCapital { get; set; }
        public string Bank1 { get; set; }
        public string Bank2 { get; set; }
        public string Bank3 { get; set; }
        public Guid RegistrationPlanId { get; set; }
        public bool IsRegistrationComplete { get; set; }
        public string RegistrationPaymentId { get; set; } = null;
        public EPaymentStatus RegistrationPaymentStatus { get; set; } = EPaymentStatus.PENDING;
 
        //navigational property
        public RegistrationPlan RegistrationPlan { get; set; }

    }
}

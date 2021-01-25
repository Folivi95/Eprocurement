using EGPS.Application.Common;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class AccountDTO: AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactEmail { get; set; }
        public string Website { get; set; }
        public string BusinessType { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string TimeZone { get; set; }
        public object CompanyLogo { get; set; }
        public Guid CreatedById { get; set; }
    }

    public class ResendAccountDTO: AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
        public string ContactEmail { get; set; }
        public string Website { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string TimeZone { get; set; }
        public string CompanyLogo { get; set; }
        public Guid UserId { get; set; }
    }
}

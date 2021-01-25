using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Common;
using EGPS.Domain.Entities;
using EGPS.Domain.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace EGPS.Application.Models.StaffModels
{
    public class StaffUserDto : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string HomeAddress { get; set; }
        public string VerificationToken { get; set; }
        public bool EmailVerified { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public object ProfilePicture { get; set; }
        public Guid MinistryId { get; set; }
        public Guid UserRoleId { get; set; } 
        public string Role { get; set; }
        public MinistryDTO Ministry { get; set; }
        public UserRoleDTO UserRole { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EStatus Status { get; set; }
        public DateTime? LastLogin { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public EUserType UserType { get; set; }
    }

    public class StaffWithTokenDto : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string HomeAddress { get; set; }
        public string VerificationToken { get; set; }
        public bool EmailVerified { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public object ProfilePicture { get; set; }
        public MinistryDTO Ministry { get; set; }
        public string Role { get; set; }
    }
}

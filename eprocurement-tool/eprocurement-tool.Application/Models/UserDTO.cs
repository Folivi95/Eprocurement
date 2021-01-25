using EGPS.Application.Common;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using EGPS.Domain.Enums;

namespace EGPS.Application.Models
{
    public class UserDTO : AuditableModelDTO
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
        public EStatus Status { get; set; }
        public int VendorRegStage { get; set; }
        public EUserType UserType { get; set; }
        public Guid? VendorProfileId { get; set; }
        public object ProfilePicture { get; set; }
        public string Role { get; set; }
        public DateTime? LastLogin { get; set; }
        public double? Threshold { get; set; }
    }

    public class UsersDTO : AuditableModelDTO
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
        public EStatus Status { get; set; }
        public EUserType UserType { get; set; }
        public object ProfilePicture { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Role { get; set; }
    }

    public class UserMemberDTO : AuditableModelDTO
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
        public EStatus Status { get; set; }
        public EUserType UserType { get; set; }
        public object ProfilePicture { get; set; }
        public ICollection<UnitMemberDTO> UnitMembers { get; set; }
        public DateTime? LastLogin { get; set; }
    }

    public class DepartmentUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class UnitUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class UserRoleDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UserForVendorProfileDTO
    {
        public VendorProfile VendorProfile { get; set; }
    }
}

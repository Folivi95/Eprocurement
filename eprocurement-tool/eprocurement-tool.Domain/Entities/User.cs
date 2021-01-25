using EGPS.Domain.Common;
using EGPS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EGPS.Domain.Entities
{
    public class User : AuditableEntity
    {
        public Guid Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string HomeAddress { get; set; }
        public string VerificationToken { get; set; }
        public bool EmailVerified { get; set; } = false;
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string ProfilePicture { get; set; }
        public Guid AccountId { get; set; }
        public EStatus Status { get; set; }
        public int VendorRegStage { get; set; }
        public EUserType UserType { get; set; }
        public DateTime? Lastlogin { get; set; }
        public Guid UserRoleId { get; set; }
        public Guid MinistryId { get; set; }
        public Guid NotificationId { get; set; }
        public ERole? Role { get; set; }
        public double? Threshold { get; set; }

        //navigation properties
        public Ministry Ministry { get; set; }
        public ICollection<DepartmentMember> DepartmentMembers { get; set; }
        public ICollection<UnitMember> UnitMembers { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<UserActivity> UserActivities { get; set; }
        public ICollection<VendorService> VendorServices { get; set; }
        public VendorProfile VendorProfile { get; set; }
        public ICollection<VendorProcurement> VendorProcurements { get; set; }
        public ICollection<Contract> Contracts { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}

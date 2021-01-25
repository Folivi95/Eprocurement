using EGPS.Application.Common;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class UserInvitationDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public bool Status { get; set; } = true;
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        public bool Deleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}

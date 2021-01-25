using EGPS.Domain.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace EGPS.Domain.Entities
{
    public class UserInvitation : AuditableEntity
    {
        public Guid Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Token { get; set; }
        public bool Status { get; set; } = true;
        public Guid AccountId { get; set; }
        public bool Deleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        //navigation property
        public Account Account { get; set; }
    }
}

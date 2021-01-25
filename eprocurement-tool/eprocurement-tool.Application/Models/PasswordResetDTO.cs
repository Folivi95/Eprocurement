using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Common;

namespace EGPS.Application.Models
{
    public class PasswordResetDTO: AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public bool Status { get; set; } 
        public Guid AccountId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class PasswordResetForCreationDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class PasswordResetLinkForCreationDTO
    {
        public string Email { get; set; }
    }
}

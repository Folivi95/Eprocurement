using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class ConfirmEmailTokenDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class ConfirmInvitationDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}

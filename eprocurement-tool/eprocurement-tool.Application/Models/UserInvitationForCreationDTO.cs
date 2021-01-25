using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class UserInvitationForCreationDTO
    {
        public int RoleId { get; set; }
        //public Guid[] CustomRoleIds { get; set; }
        public string Email { get; set; }
        public Guid MinistryId { get; set; }
    }

    public class UsersMultipleInvitesForCreationDTO
    {
        public Guid RoleId { get; set; }
        public Guid[] CustomRoleIds { get; set; }
        public string[] Emails { get; set; }
    }

}

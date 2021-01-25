using EGPS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    class StaffProfileForCreationDTO
    {
        public Guid UserID { get; set; }
        public string HomeAddress { get; set; }
        public Guid MinistryId { get; set; }
    }


    public class StaffForUpdateDTO
    {
        public Guid? MinistryId { get; set; }
        public EStatus? Status { get; set; }
        public ERole? Role { get; set; }
    }

    public class RevokeStaffInvitationRequestDTO
    {
        public string[] Emails { get; set; } = null;
    }
}

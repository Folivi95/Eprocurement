using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    class StaffProfileDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string HomeAddress { get; set; }
        public Guid MinistryId { get; set; }
    }
}

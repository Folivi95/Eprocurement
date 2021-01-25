using System;
using System.Collections.Generic;
using EGPS.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EGPS.Domain.Entities
{
    public class StaffProfile : AuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserID { get; set; }

        public string HomeAddress { get; set; }
        public Guid MinistryId { get; set; }

        //navigation property
        public User User { get; set; }
    }
}

using EGPS.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EGPS.Domain.Entities
{
    public class BusinessCategory : AuditableEntity
    {

        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Guid CreatedByID { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        //navigation property
        public User CreatedBy { get; set; }
    }
}

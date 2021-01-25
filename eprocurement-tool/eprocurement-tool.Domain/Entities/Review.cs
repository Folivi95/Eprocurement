using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EGPS.Domain.Common;
using EGPS.Domain.Enums;

namespace EGPS.Domain.Entities
{
    public class Review : AuditableEntity
    {
        public Guid Id { get; set; }
        [Required]
        public string Body { get; set; }
        public Guid ObjectId { get; set; }
        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; }
    }
}

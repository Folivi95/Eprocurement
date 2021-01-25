using EGPS.Domain.Common;
using EGPS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EGPS.Domain.Entities
{
    public class Document : AuditableEntity
    {
        public Guid Id { get; set; }
        [Required]
        public string File { get; set; }
        [Required]
        public string Name { get; set; }
        public EDocumentStatus DocumentStatus { get; set; }
        public Guid ObjectId { get; set; }
        public EDocumentObjectType ObjectType { get; set; }
        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public bool Deleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}

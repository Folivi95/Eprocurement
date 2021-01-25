using EGPS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text;
using EGPS.Domain.Common;

namespace EGPS.Domain.Entities
{
    public class Comment: AuditableEntity
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public Guid? ObjectId { get; set; }
        public string ObjectClass { get; set; }
        public CommentType Type { get; set; }
        public Comment Parent { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public bool Deleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
        public ICollection<Comment> comments { get; set; }
    }
}

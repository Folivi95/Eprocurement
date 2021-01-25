using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Common;

namespace EGPS.Application.Models
{
    public class CommentDTO: AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? ObjectId { get; set; }
        public string ObjectClass { get; set; }
        public string Type { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }

    public class CommentForCreation
    {
        public string Body { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? ObjectId { get; set; }
        public string Type { get; set; }
    }

}

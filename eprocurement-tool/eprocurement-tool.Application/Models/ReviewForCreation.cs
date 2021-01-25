using EGPS.Application.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class ReviewForCreation
    {
        public string Body { get; set; }
    }

    public class ReviewResponse: AuditableModelDTO
    {
        public string Body { get; set; }
        public Guid ObjectId { get; set; }
        public Guid CreatedById { get; set; }
        public ReviewUser CreatedBy { get; set; }
    }

    public class ReviewUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public object ProfilePicture { get; set; }
    }
}

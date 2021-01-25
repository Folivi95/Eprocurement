using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Common;

namespace EGPS.Application.Models
{
    public class DocumentClassDTO: AuditableModelDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid CreatedById { get; set; }
        public Guid AccountId { get; set; }
    }
}

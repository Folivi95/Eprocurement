using EGPS.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class BusinessCategoryForCreationDTO : AuditableModelDTO
    {
        public Guid CreatedById { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}

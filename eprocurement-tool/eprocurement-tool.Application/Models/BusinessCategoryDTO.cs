using EGPS.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class BusinessCategoryDTO : AuditableModelDTO
    {
        public Guid CreatedById { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}

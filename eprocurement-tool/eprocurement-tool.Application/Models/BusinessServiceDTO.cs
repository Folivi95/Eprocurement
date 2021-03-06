﻿using EGPS.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class BusinessServiceDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public Guid BusinessCategoryId { get; set; }
        public BusinessCategoryDTO BusinessCategory { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid CreatedById { get; set; }
    }
}

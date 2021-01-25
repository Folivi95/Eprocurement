using EGPS.Application.Common;
using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
namespace EGPS.Application.Models
{
    public class VendorRegistrationCategoryDTO: AuditableModelDTO
    {
        public Guid UserId { get; set; }
        public RegistrationPlanDTO RegistrationPlan { get; set; }
    }
}

using EGPS.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EGPS.Domain.Entities
{
    public class Account : AuditableEntity
    {
        public Guid Id { get; set; }
        [Required]
        public string CompanyName { get; set; }
        public string ContactEmail { get; set; }
        public string Website { get; set; }
        public string ContactPhone { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string BusinessType { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string TimeZone { get; set; } = "GMT+1";
        public string CompanyLogo { get; set; }
        public Guid CreatedById { get; set; }
        public ICollection<Unit> Units { get; set; }

        public ICollection<Workflow> Workflows { get; set; }
    }
}

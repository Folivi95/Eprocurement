using EGPS.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class VendorDirectorCertificateDTO : AuditableModelDTO
    {
        public Guid Id { get; set; }
        public Guid VendorDirectorId { get; set; }
        public string File { get; set; }
        public Guid CreatedById { get; set; }
    }
}

using EGPS.Application.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class VendorDirectorCertificateForCreationDTO
    {
        public IFormFile File { get; set; }
    }
}

using System;
using EGPS.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace EGPS.Application.Models
{
    public class VendorDocumentForCreationDTO
    {
        public string Description { get; set; }
        public IFormFile File { get; set; }
        public Guid VendorDocumentTypeId { get; set; }
    }
}

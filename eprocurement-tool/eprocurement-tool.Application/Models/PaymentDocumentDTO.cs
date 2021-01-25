using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace EGPS.Application.Models
{
    public class PaymentDocumentRequest
    {
        public List<IFormFile> Documents { get; set; } = new List<IFormFile>();
        public Guid ObjectId { get; set; }
    }

}
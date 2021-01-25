using System;
using System.Collections.Generic;
using EGPS.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace EGPS.Application.Models
{
    public class DocumentUploadDto
    {
        public List<IFormFile> Documents { get; set; } = new List<IFormFile>();
        public EDocumentStatus Status { get; set; }
        public Guid ObjectId { get; set; }
        public EDocumentObjectType ObjectType { get; set; }
    }

    public class DocumentDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DocumentStatus { get; set; }
        public Guid CreatedById { get; set; }
        public Guid? ObjectId { get; set; }
        public Object ObjectType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public object File { get; set; }
    }

}

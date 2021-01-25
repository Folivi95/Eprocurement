using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Domain.Common;
using EGPS.Domain.Enums;

namespace EGPS.Domain.Entities
{
    public class ProcurementPlanDocument : AuditableEntity
    {
        public Guid Id { get; set; }
        public string File { get; set; }
        public string Name { get; set; }
        public EDocumentStatus DocumentStatus { get; set; }
        public Guid? ObjectId { get; set; }
        public EDocumentObjectType ObjectType { get; set; }
        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}

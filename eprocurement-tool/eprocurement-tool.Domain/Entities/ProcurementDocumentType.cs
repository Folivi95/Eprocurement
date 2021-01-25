using EGPS.Domain.Common;
using System;

namespace EGPS.Domain.Entities
{
    public class ProcurementDocumentType : AuditableEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public Guid CreatedById { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        //navigation property
        public User User { get; set; }
    }
}

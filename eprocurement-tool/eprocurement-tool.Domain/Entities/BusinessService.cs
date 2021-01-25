using EGPS.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Domain.Entities
{
    public class BusinessService : AuditableEntity
    {

        public Guid Id { get; set; }

        public Guid BusinessCategoryID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Guid CreatedByID { get; set; }

        public bool Deleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        //navigation property
        public User CreatedBy { get; set; }

        public BusinessCategory BusinessCategory { get; set; }
        public ICollection<VendorService> VendorServices { get; set; }
    }
}

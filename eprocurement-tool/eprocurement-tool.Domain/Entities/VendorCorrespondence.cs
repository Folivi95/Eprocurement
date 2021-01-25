using EGPS.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EGPS.Domain.Entities
{
    public class VendorCorrespondence : AuditableEntity
    {

        public Guid Id { get; set; }

        //foreign key
        public Guid UserID { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }


        //navigation property
        public User User { get; set; }
    }
}

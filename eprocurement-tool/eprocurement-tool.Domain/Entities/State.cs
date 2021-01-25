using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using EGPS.Domain.Common;

namespace EGPS.Domain.Entities
{
    public class State : AuditableEntity
    {
        [Key]
        public int Id { get; set; }

        public int CountryId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
    }
}

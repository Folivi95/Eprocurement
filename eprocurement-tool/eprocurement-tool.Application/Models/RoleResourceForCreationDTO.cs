using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class RoleResourceForCreationDTO
    {
        public Guid ResourceId { get; set; }
        public object Permissions { get; set; }
    }
}

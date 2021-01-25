using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class RoleResourceDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public object Permissions { get; set; }
    }
}

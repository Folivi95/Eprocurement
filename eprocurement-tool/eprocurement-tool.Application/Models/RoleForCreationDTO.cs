using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class RoleForCreationDTO
    {
        public string Title { get; set; }
        public ICollection<RoleResourceForCreationDTO> Resources { get; set; }
    }
}

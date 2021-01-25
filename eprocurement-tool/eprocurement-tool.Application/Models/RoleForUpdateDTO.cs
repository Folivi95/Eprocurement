using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class RoleForUpdateDTO
    {
        public string Title { get; set; }
        public ICollection<ResourceForUpdateDTO> Resources { get; set; }
    }
}

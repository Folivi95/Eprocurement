using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class ResourceDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string[] Permissions { get; set; }
    }
}

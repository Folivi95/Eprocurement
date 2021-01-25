using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class ResourceForUpdateDTO
    {
        public Guid Id { get; set; }
        public object Permissions { get; set; }
    }
}

using EGPS.Application.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class BusinessServiceForCreationDTO
    {
        public List<Guid> Add { get; set; } = new List<Guid>();
        public List<Guid> Remove { get; set; } = new List<Guid>();
    }
}

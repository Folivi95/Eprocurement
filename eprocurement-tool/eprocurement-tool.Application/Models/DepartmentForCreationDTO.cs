using System;
using System.Collections.Generic;

namespace EGPS.Application.Models
{
    public class DepartmentForCreationDTO
    {
        public string Name { get; set; }
        public Guid LeadId { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
    }

    public class DepartmentForUpdateDTO
    {
        public string Name { get; set; }
        public Guid LeadId { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
    }

    public class DepartmentMembersDTO
    {
        public List<Guid> Members { get; set; }
    }
}

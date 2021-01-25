using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class UnitForCreationDTO
    {
        public string Name { get; set; }
        public Guid LeadId { get; set; }
        public Guid DepartmentId { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
    }

    public class UnitForUpdateDTO
    {
        public string Name { get; set; }
        public Guid LeadId { get; set; }
        public Guid DepartmentId { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
    }

    public class UnitMembersDTO
    {
        public List<Guid> Members { get; set; }
    }
}

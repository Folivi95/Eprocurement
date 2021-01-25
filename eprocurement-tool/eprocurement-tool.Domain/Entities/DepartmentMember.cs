using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Domain.Entities
{
    public class DepartmentMember
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
        public Department Department { get; set; }
        public Guid DepartmentId { get; set; }

    }
}

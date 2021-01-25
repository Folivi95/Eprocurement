using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Domain.Entities
{
    public class UnitMember
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
        public Unit Unit { get; set; }
        public Guid UnitId { get; set; }
    }
}

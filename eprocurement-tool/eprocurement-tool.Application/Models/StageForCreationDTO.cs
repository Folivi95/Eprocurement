using EGPS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class StageForCreationDTO
    {
        public string Title { get; set; }
        public int Index { get; set; }
        public string UserType { get; set; }
        public string GroupClass { get; set; }
        public string Action { get; set; }
        public int MinimumPass { get; set; }
        public Guid[] GroupIds { get; set; }
        public Guid[] AssigneeIds { get; set; }
    }

    public class StageForUpdateDTO
    {
        public string Title { get; set; }
        public int Index { get; set; }
        public string UserType { get; set; }
        public string GroupClass { get; set; }
        public string Action { get; set; }
        public int MinimumPass { get; set; }
        public Guid[] GroupIds { get; set; }
        public Guid[] AssigneeIds { get; set; }
    }

    public class StageForUnderWorkflowUpdateDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Index { get; set; }
        public string UserType { get; set; }
        public string GroupClass { get; set; }
        public string Action { get; set; }
        public int MinimumPass { get; set; }
        public Guid[] GroupIds { get; set; }
        public Guid[] AssigneeIds { get; set; }
    }
}

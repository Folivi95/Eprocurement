using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class WorkflowForUpdateDTO
    {
        public string Title { get; set; }
        public ICollection<StageForUnderWorkflowUpdateDTO> Stages { get; set; }
    }
}

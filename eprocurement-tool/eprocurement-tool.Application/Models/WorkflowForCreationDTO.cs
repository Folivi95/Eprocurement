using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Models
{
    public class WorkflowForCreationDTO
    {
        public string Title { get; set; }
        public StageForCreationDTO[] Stages { get; set; }
    }
}

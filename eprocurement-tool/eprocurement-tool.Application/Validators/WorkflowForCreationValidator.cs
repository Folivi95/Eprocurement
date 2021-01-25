using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Models;
using FluentValidation;

namespace EGPS.Application.Validators
{
    public class WorkflowForCreationValidator: AbstractValidator<WorkflowForCreationDTO>
    {
        public WorkflowForCreationValidator()
        {
            RuleFor(w => w.Title).NotEmpty();
        }
    }

    public class WorkflowForUpdateValidator : AbstractValidator<WorkflowForUpdateDTO>
    {
        public WorkflowForUpdateValidator()
        {
            RuleFor(w => w.Title).NotEmpty();
        }
    }
}

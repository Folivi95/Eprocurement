using EGPS.Application.Interfaces;
using EGPS.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Validators
{
    public class StageForCreationDtoValidator : AbstractValidator<StageForCreationDTO>
    {
        public StageForCreationDtoValidator()
        {
            RuleFor(x => x.Index)
                .NotEmpty().WithMessage("Enter a valid value")
                .GreaterThan(0).WithMessage("Index can not be less than 1");
            RuleFor(x => x.Title)
                .NotEmpty();
            RuleFor(x => x.UserType)
                .NotEmpty().WithMessage("User type can only be GROUP or INDIVIDUAL")
                .Must((u, s) => s.ToUpper() == "GROUP" || s.ToUpper() == "INDIVIDUAL")
                .WithMessage("User type can only be GROUP or INDIVIDUAL");
            RuleFor(x => x.GroupClass)
                .Must((u, s) => s.ToUpper() == "DEPARTMENT" || s.ToUpper() == "UNIT")
                .When(d => d.UserType == "GROUP")
                .WithMessage("Group class can only be DEPARTMENT or UNIT");

            RuleFor(x => x.Action)
                .NotEmpty().WithMessage("Action can only be CHECK or APPROVAL")
                .Must((u, s) => s.ToUpper() == "CHECK" || s.ToUpper() == "APPROVAL")
                .WithMessage("Action can only be CHECK or APPROVAL");
            
            RuleFor(x => x.MinimumPass)
                .NotEmpty().WithMessage("Enter a valid value")
                .GreaterThan(0).WithMessage("Index can not be less than 1");
        }
    }

    public class StageForUpdateDtoValidator : AbstractValidator<StageForUpdateDTO>
    {
        public StageForUpdateDtoValidator()
        {
            RuleFor(x => x.Index)
                .NotEmpty().WithMessage("Enter a valid value")
                .GreaterThan(0).WithMessage("Index can not be less than 1");
            RuleFor(x => x.Title)
                .NotEmpty();
            RuleFor(x => x.UserType)
                .NotEmpty().WithMessage("User type can only be GROUP or INDIVIDUAL")
                .Must((u, s) => s.ToUpper() == "GROUP" || s.ToUpper() == "INDIVIDUAL")
                .WithMessage("User type can only be GROUP or INDIVIDUAL");
            RuleFor(x => x.GroupClass)
                .Must((u, s) => s.ToUpper() == "DEPARTMENT" || s.ToUpper() == "UNIT")
                .When(d => d.UserType == "GROUP")
                .WithMessage("Group class can only be DEPARTMENT or UNIT");

            RuleFor(x => x.Action)
                .NotEmpty().WithMessage("Action can only be CHECK or APPROVAL")
                .Must((u, s) => s.ToUpper() == "CHECK" || s.ToUpper() == "APPROVAL")
                .WithMessage("Action can only be CHECK or APPROVAL");

            RuleFor(x => x.MinimumPass)
                .NotEmpty().WithMessage("Enter a valid value")
                .GreaterThan(0).WithMessage("Index can not be less than 1");
        }
    }

    public class StageForUpdateDtoUnderWorkflowValidator : AbstractValidator<StageForUnderWorkflowUpdateDTO>
    {
        public StageForUpdateDtoUnderWorkflowValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Index)
                .NotEmpty().WithMessage("Enter a valid value")
                .GreaterThan(0).WithMessage("Index can not be less than 1");
            RuleFor(x => x.Title)
                .NotEmpty();
            RuleFor(x => x.UserType)
                .NotEmpty().WithMessage("User type can only be GROUP or INDIVIDUAL")
                .Must((u, s) => s.ToUpper() == "GROUP" || s.ToUpper() == "INDIVIDUAL")
                .WithMessage("User type can only be GROUP or INDIVIDUAL");
            RuleFor(x => x.GroupClass)
                .Must((u, s) => s.ToUpper() == "DEPARTMENT" || s.ToUpper() == "UNIT")
                .When(d => d.UserType == "GROUP")
                .WithMessage("Group class can only be DEPARTMENT or UNIT");

            RuleFor(x => x.Action)
                .NotEmpty().WithMessage("Action can only be CHECK or APPROVAL")
                .Must((u, s) => s.ToUpper() == "CHECK" || s.ToUpper() == "APPROVAL")
                .WithMessage("Action can only be CHECK or APPROVAL");

            RuleFor(x => x.MinimumPass)
                .NotEmpty().WithMessage("Enter a valid value")
                .GreaterThan(0).WithMessage("Index can not be less than 1");
        }
    }
}

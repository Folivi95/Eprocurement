using EGPS.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Validators
{
    public class ProcurementPlanForCreationDtoValidator : AbstractValidator<ProcurementPlanForCreationDTO>
    {
        public ProcurementPlanForCreationDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.EstimatedAmountInDollars)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.EstimatedAmountInNaira)
                .NotEmpty()
                .WithMessage("Enter a valid value");
            RuleFor(x => x.ProcurementCategoryId)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.ProcurementMethodId)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.QualificationMethodId)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.MinistryId)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.PackageNumber)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.ProcessTypeId)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.ReviewMethodId)
                .NotEmpty().WithMessage("Enter a valid value");

        }
    }

    public class ProcurementPlanForUpdateDtoValidator : AbstractValidator<ProcurementPlanForUpdateDTO>
    {
        public ProcurementPlanForUpdateDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.EstimatedAmountInDollars)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.EstimatedAmountInNaira)
                .NotEmpty()
                .WithMessage("Enter a valid value");
            RuleFor(x => x.ProcurementCategoryId)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.ProcurementMethodId)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.QualificationMethodId)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.MinistryId)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.PackageNumber)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.ProcessTypeId)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.ReviewMethodId)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.GeneralPlanId)
                .NotEmpty().WithMessage("Enter a valid value");

        }
    }
}

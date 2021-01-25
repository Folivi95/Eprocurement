using EGPS.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Domain.Enums;

namespace EGPS.Application.Validators
{
    public class ActivityCreationValidator: AbstractValidator<ProcurementActivityForCreation>
    {
        public ActivityCreationValidator()
        {
            RuleForEach(x => x.Activities).ChildRules(x =>
            {
                x.RuleFor(x => x.StartDate).NotEmpty().WithMessage("Enter a valid value");
                x.RuleFor(x => x.EndDate).NotEmpty().WithMessage("Enter a valid value")
                 .GreaterThan(x => x.StartDate).WithMessage("Start date cannot be greater than end date");
                x.RuleFor(x => x.Title).NotEmpty().WithMessage("Enter a valid value");
                x.RuleFor(x => x.Index).NotEmpty().WithMessage("Enter a valid value");
                x.RuleFor(x => x.ProcurementPlanType)
                .NotEmpty().WithMessage("ProcurementPlanType can only be PROCUREMENTPLANNING or PROCUREMENTEXECUTION")
                .Must((u, s) => s.ToUpper() == "PROCUREMENTEXECUTION" || s.ToUpper() == "PROCUREMENTPLANNING")
                .WithMessage("ProcurementPlanType can only be PROCUREMENTPLANNING or PROCUREMENTEXECUTION"); ;
            });
        }
    }
}

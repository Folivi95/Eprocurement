using EGPS.Application.Models;
using FluentValidation;
using System;

namespace EGPS.Application.Validators
{
    public class MilestoneTaskForCreateDTOValidator : AbstractValidator<MilestoneTaskForCreateDTO>
    {
        public MilestoneTaskForCreateDTOValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
            RuleFor(x => x.EstimatedValue).NotEmpty().WithMessage("Estimatated value is required");
            RuleFor(ac => ac.StartDate).NotEmpty().WithMessage("Start date is required");
            RuleFor(ac => ac.EndDate).NotEmpty().WithMessage("End date is required")
                .GreaterThan(r => r.StartDate).WithMessage("Start date cannot be greater than end date");
        }
    }

    public class MilestoneInvoiceForCreationValidator : AbstractValidator<MilestoneInvoiceForCreation>
    {
        public MilestoneInvoiceForCreationValidator()
        {
            RuleFor(x => x.DueDate).NotEmpty().WithMessage("Due Date is required");
        }
    }
}

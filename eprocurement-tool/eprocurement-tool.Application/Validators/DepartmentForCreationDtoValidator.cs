using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Models;
using FluentValidation;

namespace EGPS.Application.Validators
{
    public class DepartmentForCreationDtoValidator: AbstractValidator<DepartmentForCreationDTO>
    {
        public DepartmentForCreationDtoValidator()
        {
            RuleFor(u => u.Website)
                .NotEmpty().WithMessage("Enter a valid value")
                .Matches(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$")
                .WithMessage("Enter a valid value");
            RuleFor(d => d.LeadId)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(d => d.Name)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(d => d.Description)
                .NotEmpty().WithMessage("Enter a valid value");
        }
    }

    public class DepartmentForUpdateDtoValidator : AbstractValidator<DepartmentForUpdateDTO>
    {
        public DepartmentForUpdateDtoValidator()
        {
            RuleFor(u => u.Website)
                .NotEmpty().WithMessage("Enter a valid value")
                .Matches(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$")
                .WithMessage("Enter a valid value");
            RuleFor(d => d.LeadId)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(d => d.Name)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(d => d.Description)
                .NotEmpty().WithMessage("Enter a valid value");
        }
    }
}

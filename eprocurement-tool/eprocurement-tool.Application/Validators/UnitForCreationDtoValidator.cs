using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Models;
using FluentValidation;

namespace EGPS.Application.Validators
{
    public class UnitForCreationDtoValidator: AbstractValidator<UnitForCreationDTO>
    {
        public UnitForCreationDtoValidator()
        {
            RuleFor(u => u.Name).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(u => u.DepartmentId).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(u => u.Website)
                .NotEmpty().WithMessage("Enter a valid value")
                .Matches(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$")
                .WithMessage("Enter a valid value");
            RuleFor(u => u.LeadId).NotEmpty().WithMessage("Enter a valid value");
        }
    }

    public class UnitForUpdateDtoValidator : AbstractValidator<UnitForUpdateDTO>
    {
        public UnitForUpdateDtoValidator()
        {
            RuleFor(u => u.Name).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(u => u.DepartmentId).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(u => u.Website)
                .NotEmpty().WithMessage("Enter a valid value")
                .Matches(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$")
                .WithMessage("Enter a valid value");
            RuleFor(u => u.LeadId).NotEmpty().WithMessage("Enter a valid value");
        }
    }
}

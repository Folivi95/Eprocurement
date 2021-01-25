using EGPS.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Validators
{
    public class StaffForUpdateDtoValidator: AbstractValidator<StaffForUpdateDTO>
    {
        public StaffForUpdateDtoValidator()
        {
            RuleFor(x => x.MinistryId)
                .NotEmpty()
                .WithMessage("Please enter a valid value");
            RuleFor(x => x.Role)
                .NotEmpty()
                .WithMessage("Please enter a valid value");
        }
    }
}

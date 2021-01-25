using EGPS.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Validators
{
    public class RegistrationPlanForCreationDtoValidator: AbstractValidator<RegistrationPlanForCreationDTO>
    {
        public RegistrationPlanForCreationDtoValidator()
        {
            RuleFor(x => x.TenureInDays)
                .NotEmpty()
                .WithMessage("Enter a valid value");
            RuleFor(x => x.Title)
                .NotEmpty();
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Enter a valid value");
            RuleFor(x => x.ContractMaxValue)
                .NotEmpty()
                .WithMessage("Enter a valid value");
            RuleFor(x => x.ContractMinValue)
                .NotEmpty()
                .WithMessage("Enter a valid value");
            RuleFor(x => x.Fee)
                .NotEmpty()
                .WithMessage("Enter a valid value");
            RuleFor(x => x.Grade)
                .NotEmpty()
                .WithMessage("Enter a valid value");

        }
    }
}

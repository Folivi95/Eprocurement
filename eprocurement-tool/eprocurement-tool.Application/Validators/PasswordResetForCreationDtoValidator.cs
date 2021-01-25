using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Models;
using FluentValidation;

namespace EGPS.Application.Validators
{
    public class PasswordResetForCreationDtoValidator: AbstractValidator<PasswordResetForCreationDTO>
    {
        public PasswordResetForCreationDtoValidator()
        {
            RuleFor(p => p.Email).NotEmpty().EmailAddress();
            RuleFor(p => p.Token).NotEmpty();
        }
    }

    public class PasswordResetLinkForCreationDtoValidator : AbstractValidator<PasswordResetLinkForCreationDTO>
    {
        public PasswordResetLinkForCreationDtoValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty()
                .WithMessage("Enter a valid value")
                .EmailAddress()
                .WithMessage("Enter a valid value");
        }
    }
}

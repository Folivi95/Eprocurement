using EGPS.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Validators
{
    public class UserInvitationForCreationDtoValidator : AbstractValidator<UserInvitationForCreationDTO>
    {
        public UserInvitationForCreationDtoValidator()
        {
            RuleFor(x => x.RoleId)
                .NotEmpty()
                .WithMessage("Select a valid role");
            
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }

    public class ResendInvitationMailDtoValidator : AbstractValidator<ResendInvitationMailDTO>
    {
        public ResendInvitationMailDtoValidator()
        {
            RuleFor(a => a.Email).
                NotEmpty().WithMessage("Enter a valid value").
                EmailAddress().WithMessage("Enter a valid value");
        }
    }
}

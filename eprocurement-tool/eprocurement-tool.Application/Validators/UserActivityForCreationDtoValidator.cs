using EGPS.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Validators
{
    public class UserActivityForCreationDtoValidator : AbstractValidator<UserActivityForCreationDTO>
    {
        public UserActivityForCreationDtoValidator()
        {
            RuleFor(x => x.EventType)
                .NotEmpty();
            RuleFor(x => x.ObjectClass)
                .NotEmpty();
            RuleFor(x => x.Details)
                .NotEmpty();
            RuleFor(x => x.IpAddress)
                .NotEmpty();
        }
    }
}

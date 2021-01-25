using EGPS.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Validators
{
    public class AccountForCreationDtoValidator : AbstractValidator<AccountForCreationDTO>
    {
        public AccountForCreationDtoValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty();
            RuleFor(x => x.FirstName)
                .NotEmpty();
            RuleFor(x => x.LastName)
                .NotEmpty();
            RuleFor(x => x.ContactEmail)
                .EmailAddress().NotEmpty();
            RuleFor(x => x.Website)
                .NotEmpty().Matches(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$")
                .WithMessage("Website URL not in correct format");
            RuleFor(x => x.Password)
                .NotEmpty().MinimumLength(6);
        }
    }

    public class ResendAccountMailDtoValidator : AbstractValidator<ResendAccountMailDTO>
    {
        public ResendAccountMailDtoValidator()
        {
            RuleFor(a => a.Email).
                NotEmpty().WithMessage("Enter a valid value").
                EmailAddress().WithMessage("Enter a valid value");
        }
    }

    public class AccountForUpdateValidator : AbstractValidator<AccountForUpdateDTO>
    {
        public AccountForUpdateValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.BusinessType)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.AddressLine1)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.State)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.TimeZone)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Enter a valid value")
                .Matches(@"^([0-9]{11})$" + "|" + @"^(\+[0-9]{13})$").WithMessage("Enter a valid value");
            RuleFor(x => x.ContactEmail)
                .NotEmpty().WithMessage("Enter a valid value")
                .EmailAddress().WithMessage("Enter a valid value");
            RuleFor(x => x.Website)
                .NotEmpty().WithMessage("Enter a valid value")
                .Matches(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$")
                .WithMessage("Enter a valid value");
        }
    }
}

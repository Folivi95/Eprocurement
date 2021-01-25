using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Models;
using FluentValidation;

namespace EGPS.Application.Validators
{
    public class GeneralPlanForCreationValidator: AbstractValidator<GeneralPlanForCreation>
    {
        public GeneralPlanForCreationValidator()
        {
            RuleFor(x => x.Address).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.MinistryId).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Website)
                .Matches(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$")
                .When(x => !string.IsNullOrEmpty(x.Website))
                .WithMessage("Enter a valid website");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Enter a valid value")
                .EmailAddress().WithMessage("Enter a valid value");
        }
    }

    public class GeneralPlanForUpdateValidator : AbstractValidator<GeneralPlanForUpdate>
    {
        public GeneralPlanForUpdateValidator()
        {
            RuleFor(x => x.Address).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.MinistryId).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Title).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Website)
                .Matches(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$")
                .When(x => !string.IsNullOrEmpty(x.Website))
                .WithMessage("Enter a valid website");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Enter a valid value")
                .EmailAddress().WithMessage("Enter a valid value");
        }
    }
}

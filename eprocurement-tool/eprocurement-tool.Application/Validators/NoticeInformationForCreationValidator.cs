using EGPS.Application.Models;
using EGPS.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Validators
{
    public class NoticeInformationForCreationValidator : AbstractValidator<NoticeInformationForCreation>
    {
        public NoticeInformationForCreationValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty();
            RuleFor(x => x.Description)
                .NotEmpty();
            RuleFor(x => x.Country)
                .NotEmpty();
            RuleFor(x => x.Email)
                .EmailAddress().NotEmpty();
            RuleFor(x => x.Website)
                .Matches(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$")
                .When(x => !string.IsNullOrEmpty(x.Website))
                .WithMessage("Enter a valid website");
            RuleFor(x => x.Organization)
                .NotEmpty();
            RuleFor(x => x.Title)
                .NotEmpty();
            RuleFor(x => x.PhoneNumber)
                .NotEmpty();
            RuleFor(x => x.SubmissionDeadline)
                .NotEmpty();
        }
    }
}

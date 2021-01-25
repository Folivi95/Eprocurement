using EGPS.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Validators
{
    public class NotificationForCreationDtoValidator : AbstractValidator<NotificationForCreationDTO>
    {
        public NotificationForCreationDtoValidator()
        {
            RuleFor(x => x.Body)
                .NotEmpty();
            RuleFor(x => x.Subject)
                .NotEmpty();
            RuleFor(x => x.Recipient)
                .NotEmpty();
            RuleFor(x => x.Status)
                .NotEmpty();
            RuleFor(x => x.Type)
                .NotEmpty();
            RuleFor(x => x.NotificationType)
                .NotEmpty();
            RuleFor(x => x.TemplateId)
                .NotEmpty();
            RuleFor(x => x.AccountId)
                .NotEmpty();
        }
    }
}

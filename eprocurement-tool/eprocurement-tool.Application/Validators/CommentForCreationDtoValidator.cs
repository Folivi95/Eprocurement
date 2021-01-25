using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Models;

namespace EGPS.Application.Validators
{
    public class CommentForCreationDtoValidator: AbstractValidator<CommentForCreation>
    {
        public CommentForCreationDtoValidator()
        {
            RuleFor(x => x.Body).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Enter a valid value")
                .Must((u, s) => s.ToUpper() == "SUGGESTION" || s.ToUpper() == "COMPLAINT" || s.ToUpper() == "COMMENT").WithMessage("Comment type can only be suggestion, complaint and comment");
            RuleFor(x => x.Email).EmailAddress()
                .When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("Enter a valid value");


        }
    }
}

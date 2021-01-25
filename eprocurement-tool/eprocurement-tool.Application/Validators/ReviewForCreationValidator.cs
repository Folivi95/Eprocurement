using EGPS.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Validators
{
    public class ReviewForCreationValidator : AbstractValidator<ReviewForCreation>
    {
        public ReviewForCreationValidator()
        {
            RuleFor(x => x.Body).NotEmpty().WithMessage("Enter a valid value");
        }
    }
}

using EGPS.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Validators
{
    public class DocumentDatasheetForCreationValidator : AbstractValidator<DocumentDatasheetCreation>
    {
        public DocumentDatasheetForCreationValidator()
        {
            RuleFor(x => x.SubmissionDeadline).NotEmpty().WithMessage("Enter a valid value");
        }
    }
}


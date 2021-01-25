using EGPS.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Validators
{
    public class ContractSigningDocumentForCreationValidator : AbstractValidator<ContractSigningDocumentAndDatasheetCreation>
    {
        public ContractSigningDocumentForCreationValidator()
        {
            RuleFor(x => x.SignatureDate).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Reference).NotEmpty().WithMessage("Enter a valid value");
        }
    }
}

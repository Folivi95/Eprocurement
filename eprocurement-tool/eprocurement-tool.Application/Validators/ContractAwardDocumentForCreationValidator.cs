using EGPS.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Validators
{
    public class ContractAwardDocumentForCreationValidator : AbstractValidator<ContractAwardDocumentCreation>
    {
        public ContractAwardDocumentForCreationValidator()
        {
            RuleFor(x => x.IssuedDate).NotEmpty().WithMessage("Enter a valid value");

            //RuleFor(x => x.EndDate).NotEmpty().WithMessage("Enter a valid value");
        }
    }
}

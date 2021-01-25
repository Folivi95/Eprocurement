using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Models;
using FluentValidation;

namespace EGPS.Application.Validators
{
    public class DocumentUploadValidator: AbstractValidator<DocumentUploadDto>
    {
        public DocumentUploadValidator()
        {
            RuleFor(x => x.Documents).NotEmpty().WithMessage("A document must be uploaded");
            RuleFor(x => x.Status).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.ObjectId).NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.ObjectType).NotEmpty().WithMessage("Enter a valid value");
        }
    }
}

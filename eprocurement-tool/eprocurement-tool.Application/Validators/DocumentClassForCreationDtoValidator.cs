using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Models;
using FluentValidation;

namespace EGPS.Application.Validators
{
    public class DocumentClassForCreationDtoValidator: AbstractValidator<DocumentClassForCreationDTO>
    {
        public DocumentClassForCreationDtoValidator()
        {
            RuleFor(d => d.Title)
                .NotEmpty()
                .WithMessage("Enter a valid value");
        }
    }

    public class DocumentClassForUpdateDtoValidator : AbstractValidator<DocumentClassForUpdateDTO>
    {
        public DocumentClassForUpdateDtoValidator()
        {
            RuleFor(d => d.Title)
                .NotEmpty()
                .WithMessage("Enter a valid value");
        }
    }
}

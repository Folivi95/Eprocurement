using EGPS.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Validators
{
    public class VendorDocumentForCreationDtoValidator: AbstractValidator<VendorDocumentForCreationDTO>
    {
        public VendorDocumentForCreationDtoValidator()
        {
            RuleFor(x => x.VendorDocumentTypeId).NotEmpty()
                .WithMessage("Enter a value for vendorDocumentTypeId");
            RuleFor(x => x.File).NotEmpty().WithMessage("Supply a value for file");
        }
    }
}

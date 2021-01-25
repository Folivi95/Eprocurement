using EGPS.Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EGPS.Application.Validators
{
    public class VendorDirectorForCreationDtoValidator: AbstractValidator<VendorDirectorForCreationDTO>
    {
        public VendorDirectorForCreationDtoValidator()
        {
            RuleFor(x => x.AddressLine1)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.IdentificationType)
                .NotEmpty()
                .WithMessage("Enter a valid value");
            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("Enter a valid value");
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Enter a valid value");
            RuleFor(x => x.Country) 
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.State)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Certifications)
                .NotEmpty().WithMessage("Please upload at least one certificate file");
            RuleFor(x => x.PassportPhoto)
                .NotEmpty().WithMessage("Please provide a valid image");
            RuleFor(x => x.IdentificationFile)
                .NotEmpty().WithMessage("Please provide a valid file");
        }
        
    }


    public class VendorDirectorForUpdateDtoValidator: AbstractValidator<VendorDirectorForUpdateDTO>
    {
        public VendorDirectorForUpdateDtoValidator()
        {
            RuleFor(x => x.AddressLine1)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("Enter a valid value");
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Enter a valid value");
            RuleFor(x => x.Country) 
                .NotEmpty().WithMessage("Enter a valid Country");
            RuleFor(x => x.State)
                .NotEmpty().WithMessage("Enter a valid State");
        }
        
    }
}

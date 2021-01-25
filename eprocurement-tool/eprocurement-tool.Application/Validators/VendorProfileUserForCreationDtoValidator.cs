using System;
using EGPS.Application.Models;
using FluentValidation;

namespace eprocurement_tool.Application.Validators
{
    public class VendorProfileUserForCreationDtoValidator : AbstractValidator<VendorProfileUserForCreationDTO>
    {
        public VendorProfileUserForCreationDtoValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty();
            RuleFor(x => x.CompanyPhoneNumber)
                .NotEmpty()
                .WithMessage("Enter a valid value");
            RuleFor(x => x.AddressLine1)
                .NotEmpty()
                .WithMessage("Enter a valid value");
            RuleFor(x => x.AddressLine2)
                .NotEmpty();
            RuleFor(x => x.City)
                .NotEmpty();
            RuleFor(x => x.FirstName)
                .NotEmpty();
            RuleFor(x => x.LastName)
                .NotEmpty();
            RuleFor(x => x.Email)
                .EmailAddress().NotEmpty()
                .WithMessage("Enter a valid value");
            RuleFor(x => x.Password)
                .Matches(@"(?-i)(?=^.{8,}$)((?!.*\s)(?=.*[A-Z])(?=.*[a-z]))((?=(.*\d){1,})|(?=(.*\W){1,}))^.*$")
                .WithMessage(@"Password must be atleast 8 characters, Atleast 1 upper case letters (A – Z), Atleast 1 lower case letters (a – z), Atleast 1 number (0 – 9) or non-alphanumeric symbol (e.g. @ '$%£! ')");
            
        }
    }

    public class VendorProfileForUpdateDtoValidator : AbstractValidator<VendorProfileForUpdateDTO>
    {
        public VendorProfileForUpdateDtoValidator()
        {
            RuleFor(x => x.CompanyName)
               .NotEmpty();
            RuleFor(x => x.CompanyPhoneNumber)
                .NotEmpty()
                .WithMessage("Enter a valid value");
            RuleFor(x => x.AddressLine1)
                .NotEmpty()
                .WithMessage("Enter a valid value");
            RuleFor(x => x.AddressLine2)
                .NotEmpty();
            RuleFor(x => x.City)
                .NotEmpty();
            RuleFor(x => x.FirstName)
                .NotEmpty();
            RuleFor(x => x.LastName)
                .NotEmpty();
            RuleFor(x => x.CoreCompetency)
                .NotEmpty()
                .WithMessage("Select a valid option");
            RuleFor(x => x.OrganizationType)
                .NotEmpty()
                .WithMessage("Select a valid option");
            RuleFor(x => x.IncorporationDate)
                .NotEmpty().WithMessage("Enter a valid value")
                .Must((vp, i) => vp.IncorporationDate <= DateTime.UtcNow)
                .WithMessage("You can not enter a date greater than today")
                ;
            RuleFor(x => x.Website)
                .NotEmpty().Matches(@"^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$")
                .WithMessage("Website URL not in correct format");

        }
    }
}

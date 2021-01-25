using EGPS.Application.Models;
using FluentValidation;

namespace EGPS.Application.Validators
{
    public class VendorProfileForCreationDtoValidator : AbstractValidator<VendorProfileForCreationDTO>
    {
        public VendorProfileForCreationDtoValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.OrganizationType)
                .NotEmpty();
            RuleFor(x => x.AddressLine1)
                .NotEmpty()
                .WithMessage("Enter a valid value");
            RuleFor(x => x.AddressLine2)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.State)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Website)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.CoreCompetency)
                .NotEmpty();
            RuleFor(x => x.AuthorizedShareCapital)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.IncorporationDate)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.CACRegistrationNumber)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.AuthorizedShareCapital)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.CorrespondenceCountry)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.CorrespondenceState)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.CorrespondenceAddress2)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.CorrespondenceAddress1)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.CorrespondenceCity)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.ContactPhoneNumber)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.ContactLastName)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.ContactPosition)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.ContactFirstName)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.ContactEmail)
                .NotEmpty().EmailAddress().WithMessage("Enter a valid value");
        }
    }
}

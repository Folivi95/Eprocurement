using EGPS.Application.Models;
using FluentValidation;

namespace EGPS.Application.Validators
{
    public class VendorAttestationForCreationDtoValidator : AbstractValidator<VendorAttestationForCreationDTO>
    {
        public VendorAttestationForCreationDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Enter a valid value");
            RuleFor(x => x.AttestedAt)
                .NotEmpty();
        }
    }
}

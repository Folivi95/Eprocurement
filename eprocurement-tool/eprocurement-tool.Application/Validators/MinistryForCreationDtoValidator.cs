using EGPS.Application.Models;
using FluentValidation;

namespace EGPS.Application.Validators
{
    public class MinistryForCreationDtoValidator : AbstractValidator<MinistryForCreationDTO>
    {
        public MinistryForCreationDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Enter a valid value");
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Enter a valid value");
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Enter a valid email");
        }
    }
}

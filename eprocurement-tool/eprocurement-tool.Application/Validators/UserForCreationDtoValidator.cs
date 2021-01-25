using EGPS.Application.Models;
using FluentValidation;
using System.Data;
using System.Linq;

namespace EGPS.Application.Validators
{
    public class UserForCreationDtoValidator : AbstractValidator<UserForCreationDTO>
    {
        public UserForCreationDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().Matches("^[a-zA-Z0-9]*$");
            RuleFor(x => x.LastName)
                .NotEmpty().Matches("^[a-zA-Z0-9]*$");
            RuleFor(x => x.Email)
                .NotEmpty().EmailAddress();
            RuleFor(x => x.Phone)
                .NotEmpty();
            RuleFor(x => x.Token)
                .NotEmpty();
            RuleFor(x => x.Password)
                .Matches(@"(?-i)(?=^.{8,}$)((?!.*\s)(?=.*[A-Z])(?=.*[a-z]))((?=(.*\d){1,})|(?=(.*\W){1,}))^.*$")
                .WithMessage(@"Password must be atleast 8 characters, Atleast 1 upper case letters (A – Z), Atleast 1 lower case letters (a – z), Atleast 1 number (0 – 9) or non-alphanumeric symbol (e.g. @ '$%£! ')");
        }
    }

    public class UserLoginForCreationDtoValidator : AbstractValidator<UserLoginForCreationDTO>
    {
        public UserLoginForCreationDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Enter a valid value")
                .EmailAddress()
                .WithMessage("Enter a valid value");
            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }

    public class UserChangePasswordForCreationDtoValidator : AbstractValidator<UserChangePasswordForCreationDTO>
    {
        public UserChangePasswordForCreationDtoValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty();
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("Enter a valid value")
                .Matches(@"(?-i)(?=^.{8,}$)((?!.*\s)(?=.*[A-Z])(?=.*[a-z]))((?=(.*\d){1,})|(?=(.*\W){1,}))^.*$")
                .WithMessage(@"Password must be atleast 8 characters, Atleast 1 upper case letters (A – Z), Atleast 1 lower case letters (a – z), Atleast 1 number (0 – 9) or non-alphanumeric symbol (e.g. @ '$%£! ')");
        }
    }
    public class UserForUpdateDtoValidator : AbstractValidator<UserForUpdateDTO>
    {
        public UserForUpdateDtoValidator()
        {
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Enter a valid value")
                .Matches(@"^([0-9]{11})$" + "|" + @"^(\+[0-9]{13})$").WithMessage("Enter a valid value");
            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Enter a valid value")
                .Must((u, s) => s.ToLower() =="male" || s.ToLower() == "female")
                .WithMessage("Enter a valid value");
        }
    }

    public class UsersMultipleInvitesForCreationDtoValidator : AbstractValidator<UsersMultipleInvitesForCreationDTO>
    {
        public UsersMultipleInvitesForCreationDtoValidator()
        {
            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("Enter a valid value");
            RuleFor(x => x.Emails)
                .Must(e => e.Length > 0)
                .WithMessage("Email list must be at least one");
            RuleForEach(x => x.Emails)
                .EmailAddress().WithMessage("Enter a valid value");
        }
    }


    public class UserVendorForCreationDTOValidator : AbstractValidator<UserVendorForCreationDTO>
    {
        public UserVendorForCreationDTOValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().Matches("^[a-zA-Z0-9]*$");
            RuleFor(x => x.LastName)
                .NotEmpty().Matches("^[a-zA-Z0-9]*$");
            RuleFor(x => x.Email)
                .NotEmpty().EmailAddress();
            RuleFor(x => x.Password)
                .Matches(@"(?-i)(?=^.{8,}$)((?!.*\s)(?=.*[A-Z])(?=.*[a-z]))((?=(.*\d){1,})|(?=(.*\W){1,}))^.*$")
                .WithMessage(@"Password must be atleast 8 characters, Atleast 1 upper case letters (A – Z), Atleast 1 lower case letters (a – z), Atleast 1 number (0 – 9) or non-alphanumeric symbol (e.g. @ '$%£! ')");
        }
    }
}

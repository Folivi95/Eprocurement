using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EGPS.Application.Models;
using FluentValidation;

namespace EGPS.Application.Validators
{
    public class RoleForCreationDtoValidator: AbstractValidator<RoleForCreationDTO>
    {
        public RoleForCreationDtoValidator()
        {
            RuleFor(r => r.Title).NotEmpty();
            RuleForEach(r => r.Resources).ChildRules(x =>
            {
                x.RuleFor(x => x.Permissions).NotEmpty().WithMessage("You must provide permissions");
                x.RuleFor(x => x.ResourceId).NotEmpty().WithMessage("You must provide a resouceId");
            })
;       }
    }
}

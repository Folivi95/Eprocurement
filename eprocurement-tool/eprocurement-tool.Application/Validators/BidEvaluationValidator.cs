using System;
using System.Collections.Generic;
using System.Text;
using EGPS.Application.Models;
using FluentValidation;

namespace EGPS.Application.Validators
{
    public class BidEvaluationValidator : AbstractValidator<BidEvaluationForCreation>
    {

        public BidEvaluationValidator()
        {
            RuleFor(b => b.AddedVendors).NotNull().When(x => x.AddedVendors.Count == 0);
            RuleFor(b => b.RemovedVendors).NotNull().When(x => x.AddedVendors.Count == 0);

        }
    }
}

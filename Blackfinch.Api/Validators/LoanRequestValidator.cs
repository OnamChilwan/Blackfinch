using Blackfinch.Api.Models;
using FluentValidation;

namespace Blackfinch.Api.Validators;

public class LoanRequestValidator : AbstractValidator<LoanRequest>
{
    public LoanRequestValidator()
    {
        RuleFor(x => x.CreditScore)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(999);

        RuleFor(x => x.AssetValue)
            .GreaterThan(0);

        RuleFor(x => x.LoanAmount)
            .GreaterThan(0);
    }
}
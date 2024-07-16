using Blackfinch.Api.Models;
using Blackfinch.Api.Validators;
using FluentValidation.TestHelper;

namespace Blackfinch.Api.UnitTests.Validators;

public class LoanRequestValidatorTests
{
    private LoanRequestValidator _subject = null!;
    private LoanRequest _request = null!;

    [SetUp]
    public void Setup()
    {
        _subject = new LoanRequestValidator();
        _request = new LoanRequest { CreditScore = 1, AssetValue = 1, LoanAmount = 1 };
    }

    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(1000)]
    public void Should_Have_Error_When_Credit_Is_Invalid(int creditScore)
    {
        _request.CreditScore = creditScore;
        var result = _subject.TestValidate(_request);
        result.ShouldHaveValidationErrorFor(x => x.CreditScore);
    }
    
    [Test]
    public void Should_Have_Error_When_Loan_Amount_Is_Invalid()
    {
        _request.LoanAmount = 0;
        var result = _subject.TestValidate(_request);
        result.ShouldHaveValidationErrorFor(x => x.LoanAmount);
    }
    
    [Test]
    public void Should_Have_Error_When_Asset_Value_Is_Invalid()
    {
        _request.AssetValue = 0;
        var result = _subject.TestValidate(_request);
        result.ShouldHaveValidationErrorFor(x => x.AssetValue);
    }
}
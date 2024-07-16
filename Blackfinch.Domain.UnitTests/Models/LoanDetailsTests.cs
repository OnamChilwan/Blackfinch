using Blackfinch.Domain.Models;
using FluentAssertions;

namespace Blackfinch.Domain.UnitTests.Models;

public class LoanDetailsTests
{
    [TestCase(80, 100, 750, 80)]
    public void Given_Valid_Data_Then_Loan_Details_Successfully_Created(decimal loanAmount, decimal assetValue, int creditScore, decimal expectedLoanToValue)
    {
        var subject = new LoanDetails("123", loanAmount, assetValue, creditScore);
        
        subject.LoanToValue.Should().Be(expectedLoanToValue);
        subject.AssetValue.Should().Be(assetValue);
        subject.Amount.Should().Be(loanAmount);
        subject.CreditScore.Should().Be(creditScore);
    }

    [TestCase("")]
    [TestCase(" ")]
    [TestCase(null)]
    public void Given_Invalid_ApplicantId_Then_Exception_Is_Thrown(string applicantId)
    {
        Assert.Throws<ArgumentException>(() => new LoanDetails(applicantId, 1, 1, 1));
    }
    
    [Test]
    public void Given_Invalid_Amount_Then_Exception_Is_Thrown()
    {
        Assert.Throws<ArgumentException>(() => new LoanDetails("123", 0, 1, 1));
    }
    
    [Test]
    public void Given_Invalid_Asset_Value_Then_Exception_Is_Thrown()
    {
        Assert.Throws<ArgumentException>(() => new LoanDetails("123", 1, 0, 1));
    }
    
    [Test]
    public void Given_Invalid_Credit_Score_Then_Exception_Is_Thrown()
    {
        Assert.Throws<ArgumentException>(() => new LoanDetails("123", 1, 1, 0));
    }
}
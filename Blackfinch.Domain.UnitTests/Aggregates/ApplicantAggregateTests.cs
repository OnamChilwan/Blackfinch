using Blackfinch.Domain.Aggregates;
using Blackfinch.Domain.Events;
using Blackfinch.Domain.Exceptions;
using Blackfinch.Domain.Models;
using FluentAssertions;

namespace Blackfinch.Domain.UnitTests.Aggregates;

public class ApplicantAggregateTests
{
    private ApplicantAggregate _subject = null!;

    [SetUp]
    public void Setup()
    {
        _subject = new ApplicantAggregate();
    }

    [TestCase(200000, 350000, 750)] // 57% LTV
    [TestCase(200000, 260000, 800)] // 76% LTV
    [TestCase(200000, 225000, 900)] // 88% LTV
    public void Given_Valid_Loan_Details_When_Applying_Loan_Then_Loan_Is_Successful(decimal loanAmount, decimal assetValue, int creditScore)
    {
        _subject.ApplyForLoan(new LoanDetails("123", loanAmount, assetValue, creditScore));
        var @event = _subject.UncommittedEvents.Single(x => x is LoanApplicationSucceeded) as LoanApplicationSucceeded;

        @event.Should().NotBeNull();
        @event?.ApplicantId.Should().Be("123");
        @event?.Amount.Should().Be(loanAmount);
    }

    [Test]
    public void Given_Applicant_Exists_With_Multiple_Applications_Then_Applications_Are_Saved()
    {
        var events = new List<object>
        {
            new LoanDeclined("123", "Reason", 1000, 60),
            new LoanApplicationSucceeded("123", 100, 10),
            new LoanApplicationSucceeded("123", 200, 10),
            new LoanApplicationSucceeded("123", 300, 10),
        };

        _subject = new ApplicantAggregate(events);
        _subject.Id.Should().Be("123");
        _subject.TotalNumberOfApplications().Should().Be(4);
        _subject.TotalValueOfLoans().Should().Be(600);
        _subject.AverageLoanToValue().Should().Be(10);
        _subject.UncommittedEvents.Count.Should().Be(0);
    }

    [TestCase(99999)]
    [TestCase(1500001)]
    public void Given_Invalid_Loan_Amount_Then_Loan_Is_Declined(decimal amount)
    {
        _subject.ApplyForLoan(new LoanDetails("123", amount, 1000, 100));
        var @event = _subject.UncommittedEvents.Single(x => x is LoanDeclined) as LoanDeclined;

        @event.Should().NotBeNull();
        @event?.ApplicantId.Should().Be("123");
        @event?.Amount.Should().Be(amount);
    }

    [Test]
    public void Given_Loan_Amount_Exceeds_Million_And_Poor_Credit_Score_Then_Loan_Is_Declined()
    {
        const decimal million = 1000000;
        const decimal assetValue = 1300000;
        
        _subject.ApplyForLoan(new LoanDetails("123", million, assetValue, 949));
        var @event = _subject.UncommittedEvents.Single(x => x is LoanDeclined) as LoanDeclined;

        @event.Should().NotBeNull();
        @event?.ApplicantId.Should().Be("123");
        @event?.Amount.Should().Be(million);
    }

    [Test]
    public void Given_Loan_To_Value_That_Exceeds_Threshold_Then_Exception_Is_Thrown()
    {
        Assert.Throws<InvalidLoanApplicationException>(() => _subject.ApplyForLoan(new LoanDetails("123", 900000, 100000, 999)));
    }
}
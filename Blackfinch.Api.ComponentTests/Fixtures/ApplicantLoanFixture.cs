using Blackfinch.Api.ComponentTests.Steps;
using Blackfinch.Api.Models;
using TestStack.BDDfy;

namespace Blackfinch.Api.ComponentTests.Fixtures;

public class ApplicantLoanFixture
{
    private const string Id = "123";
    private ApplyLoanSteps _steps = null!;

    [SetUp]
    public void Setup()
    {
        _steps = new ApplyLoanSteps();
    }
    
    [Test]
    public void Given_A_New_Applicant_When_Valid_Request_Is_Sent_Then_Created_Response_Is_Returned()
    {
        var request = new LoanRequest
        {
            AssetValue = 350000,
            CreditScore = 950,
            LoanAmount = 200000
        };
        
        this.Given(_ => _steps.ApplicantDoesNotExist(Id))
            .When(_ => _steps.RequestIsSent(Id, request))
            .Then(_ => _steps.CreatedResponseIsReturned())
            .And(_ => _steps.AggregateIsSaved(Id))
            .And(_ => _steps.LoanResponseIsCorrect(new LoanResponse
            {
                TotalNumberOfApplications = 1,
                TotalLoanAmount = request.LoanAmount,
                AverageLoanToValue = 57,
                Success = true
            }))
            .BDDfy(); 
    }
    
    [Test]
    public void Given_Invalid_Request_When_Request_Is_Sent_Then_Bad_Request_Is_Returned()
    {
        var request = new LoanRequest
        {
            AssetValue = 0,
            CreditScore = 0,
            LoanAmount = 0
        };
        
        this.Given(_ => _steps.ApplicantDoesNotExist(Id))
            .When(_ => _steps.RequestIsSent(Id, request))
            .Then(_ => _steps.BadRequestResponseIsReturned())
            .BDDfy();
    }
    
    [Test]
    public void Given_An_Invalid_Loan_Request_When_Request_Is_Sent_Then_Unprocessable_Response_Is_Returned()
    {
        var request = new LoanRequest
        {
            AssetValue = 2000000,
            CreditScore = 950,
            LoanAmount = 100
        };
        
        this.Given(_ => _steps.ApplicantDoesNotExist(Id))
            .When(_ => _steps.RequestIsSent(Id, request))
            .Then(_ => _steps.UnprocessableResponseIsReturned())
            .And(_ => _steps.AggregateIsSaved(Id))
            .And(_ => _steps.LoanResponseIsCorrect(new LoanResponse
            {
                TotalNumberOfApplications = 1,
                TotalLoanAmount = 0,
                AverageLoanToValue = 0,
                Success = false
            }))
            .BDDfy();
    }
}
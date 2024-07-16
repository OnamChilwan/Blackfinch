using Blackfinch.Api.ComponentTests.Steps;
using Blackfinch.Api.Models;
using TestStack.BDDfy;

namespace Blackfinch.Api.ComponentTests.Fixtures;

public class ApplicantLoanFixture
{
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
            Id = "123",
            AssetValue = 350000,
            CreditScore = 950,
            LoanAmount = 200000
        };
        
        this.Given(_ => _steps.RequestOf(request))
            .When(_ => _steps.RequestIsSent())
            .Then(_ => _steps.CreatedResponseIsReturned())
            .BDDfy(); 
    }
}
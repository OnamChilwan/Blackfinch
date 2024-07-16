using Blackfinch.Api.ComponentTests.Steps;
using Blackfinch.Api.Models;
using TestStack.BDDfy;

namespace Blackfinch.Api.ComponentTests.Fixtures;

public class ApplicantLoanFixture
{
    private const string id = "123";
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
        
        this.Given(_ => _steps.RequestOf(request))
            .When(_ => _steps.RequestIsSent(id))
            .Then(_ => _steps.CreatedResponseIsReturned())
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
        
        this.Given(_ => _steps.RequestOf(request))
            .When(_ => _steps.RequestIsSent(id))
            .Then(_ => _steps.BadRequestResponseIsReturned())
            .BDDfy();
    }
}